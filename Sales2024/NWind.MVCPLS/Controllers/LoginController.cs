using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using proxy;
using Entities;
using System.Threading.Tasks;
using System.Net.Http;

namespace NWind.MVCPLS.Controllers
{
    public class LoginController : Controller
    {
        private readonly Proxy _proxy;

        public LoginController()
        {
            // Crear una instancia de HttpClient
            HttpClient client = new HttpClient { BaseAddress = new Uri("http://localhost:5123/api") }; // Asegúrate de poner la URL correcta de tu API
            _proxy = new Proxy(client);
        }

        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        // GET: Formulario de Crear Usuario
        public ActionResult CreateUser()
        {
            return View();
        }

        public ActionResult VerifyAccount()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> Index(LoginRequest loginRequest)
        {
            // Usar el Proxy para obtener el token
            var token = await _proxy.LoginUserAsync(loginRequest);

            if (string.IsNullOrEmpty(token))
            {
                return Json(new { success = false, message = "Login failed. Token is empty." });
            }

            return Json(new { success = true, token, redirectUrl = Url.Action("Home", "Home") });
        }

        [HttpPost]
        public async Task<JsonResult> CreateUser(Users user)
        {
            try
            {
                var proxy = new Proxy();

                user.rol = "VIEWER"; // Asegúrate de que el rol sea siempre "VIEWER"

                var createdUser = await proxy.CreateUser(user);

                if (createdUser != null)
                {
                    // Enviar la URL de redirección en la respuesta JSON
                    return Json(new { success = true, redirectUrl = Url.Action("VerifyAccount") });
                }

                // Si no se pudo crear el usuario, agrega un mensaje de error
                return Json(new { success = false, message = "No se pudo crear el usuario. Intente nuevamente." });
            }
            catch (Exception ex)
            {
                // Enviar el error en la respuesta JSON
                return Json(new { success = false, message = "Ocurrió un error: " + ex.Message });
            }
        }



    }
}
