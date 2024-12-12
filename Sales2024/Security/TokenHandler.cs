using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Security
{
    public class TokenHandler : DelegatingHandler
    {
        private readonly List<string> _excludedPaths = new List<string>
        {
            "/api/User/Login",
            "/api/User/Create",
            // Agrega aquí más rutas que quieras excluir
        };

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Obtener la ruta desde la URI
            var uri = request.RequestUri.AbsolutePath;
            var segments = uri.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            string controllerContext = null;
            string actionContext = null;

            if (segments.Length >= 2)
            {
                controllerContext = segments[1];
                if (segments.Length >= 3)
                    actionContext = segments[2];
            }

            // Verificar si la ruta está en la lista de excluidas o tiene el atributo PublicRoute
            if (_excludedPaths.Any(path => uri.Contains(path)) ||
                IsAnonymousRoute(controllerContext, actionContext))
            {
                return await base.SendAsync(request, cancellationToken);
            }

            // Obtener y validar el token
            string token = Auth.GetTokenFromRequest(request);
            if (string.IsNullOrEmpty(token))
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
            }

            if (!Auth.IsTokenValid(token))
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
            }

            // Obtener información del usuario del token
            string username = Auth.GetUsernameFromToken(token);
            string rol = Auth.GetRolFromToken(token);

            // Guardar información en el contexto
            SessionContext.Username = username;
            SessionContext.Rol = rol;

            // Verificar los roles requeridos
            if (!HasRequiredRoles(controllerContext, actionContext, new List<string> { rol }))
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.Forbidden);
            }

            return await base.SendAsync(request, cancellationToken);
        }

        private bool IsAnonymousRoute(string controllerName, string actionName)
        {
            if (string.IsNullOrEmpty(controllerName))
                return false;

            var controllerType = GetControllerType(controllerName);
            if (controllerType == null)
                return false;

            // Si no hay actionName, verificar solo el controlador
            if (string.IsNullOrEmpty(actionName))
            {
                return controllerType.GetCustomAttributes(typeof(PublicRouteAttribute), true).Any();
            }

            var methodInfo = controllerType.GetMethod(actionName);
            if (methodInfo == null)
                return false;

            var isAnonymousController = controllerType.GetCustomAttributes(typeof(PublicRouteAttribute), true).Any();
            var isAnonymousAction = methodInfo.GetCustomAttributes(typeof(PublicRouteAttribute), true).Any();

            return isAnonymousController || isAnonymousAction;
        }

        private bool HasRequiredRoles(string controllerName, string actionName, List<string> userRoles)
        {
            var controllerType = GetControllerType(controllerName);
            if (controllerType == null)
                return true;

            var methodInfo = string.IsNullOrEmpty(actionName) ? null : controllerType.GetMethod(actionName);

            var controllerRoles = controllerType.GetCustomAttributes(typeof(AuthorizeRolesAttribute), true)
                .Cast<AuthorizeRolesAttribute>()
                .SelectMany(attr => attr.Roles)
                .ToList();

            var actionRoles = methodInfo?.GetCustomAttributes(typeof(AuthorizeRolesAttribute), true)
                .Cast<AuthorizeRolesAttribute>()
                .SelectMany(attr => attr.Roles)
                .ToList() ?? new List<string>();

            if (!controllerRoles.Any() && !actionRoles.Any())
                return true;

            var requiredRoles = actionRoles.Any() ? actionRoles : controllerRoles;
            return requiredRoles.Any(role => userRoles.Contains(role, StringComparer.OrdinalIgnoreCase));
        }

        private Type GetControllerType(string controllerName)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .FirstOrDefault(t => t.Name.Equals($"{controllerName}Controller", StringComparison.OrdinalIgnoreCase));
        }
    }
    

    // Atributo para marcar rutas públicas (si no quieres usar la lista _excludedPaths)
    public class PublicRouteAttribute : Attribute
    {
    }
}