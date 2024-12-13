using Entities;
using proxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace NWind.MVCPLS.Controllers
{
    public class UserController : Controller
    {
        private Proxy _proxy;

        // Vista para gestionar todos los usuarios
        public ActionResult ManageUsers()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> GetAllUsers()
        {
            try
            {
                string authToken = Request.Headers["Authorization"];

                if (string.IsNullOrEmpty(authToken))
                {
                    return Json(new { success = false, message = "No se encontró el token de autenticación." }, JsonRequestBehavior.AllowGet);
                }

                updateProxy(authToken);

                var users = await _proxy.GetAllUsersAsync();

                return Json(users, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al obtener los usuarios: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateUser(Users user)
        {
            try
            {
                string authToken = Request.Headers["Authorization"];

                if (string.IsNullOrEmpty(authToken))
                {
                    return Json(new { success = false, message = "No se encontró el token de autenticación." });
                }

                updateProxy(authToken);

                var createdUser = await _proxy.CreateUserAsync(user);
                if (createdUser != null)
                {
                    return Json(new { success = true, message = "Usuario creado exitosamente", user = createdUser });
                }

                return Json(new { success = false, message = "No se pudo crear el usuario." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult> UpdateUser(Users user)
        {
            try
            {
                string authToken = Request.Headers["Authorization"];

                if (string.IsNullOrEmpty(authToken))
                {
                    return Json(new { success = false, message = "No se encontró el token de autenticación." });
                }

                updateProxy(authToken);

                var success = await _proxy.UpdateUserAsync(user);
                if (success)
                {
                    return Json(new { success = true, message = "Usuario actualizado exitosamente" });
                }

                return Json(new { success = false, message = "No se pudo actualizar el usuario." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult> ChangeUserStatus(int userId)
        {
            try
            {
                string authToken = Request.Headers["Authorization"];

                if (string.IsNullOrEmpty(authToken))
                {
                    return Json(new { success = false, message = "No se encontró el token de autenticación." });
                }

                updateProxy(authToken);

                var success = await _proxy.ChangeUserStatusAsync(userId);
                if (success)
                {
                    return Json(new { success = true, message = "Estado del usuario cambiado exitosamente" });
                }

                return Json(new { success = false, message = "No se pudo cambiar el estado del usuario." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }

        private void updateProxy(string token)
        {
            HttpClient client = new HttpClient { BaseAddress = new Uri("http://localhost:5123/api") }; // Cambia a la URL correcta
            client.DefaultRequestHeaders.Add("Authorization", token);
            _proxy = new Proxy(client);
        }
    }
}