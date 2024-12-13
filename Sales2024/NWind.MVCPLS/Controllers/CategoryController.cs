using Entities;
using Newtonsoft.Json.Linq;
using proxy;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace NWind.MVCPLS.Controllers
{
    public class CategoryController : Controller
    {
        private Proxy _proxy;

        // Vista para gestionar todas las categorías
        public ActionResult ManageCategories()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> GetAllCategories()
        {
            try
            {
                // Recuperar el token de la sesión (del encabezado Authorization)
                string authToken = Request.Headers["Authorization"];

                if (string.IsNullOrEmpty(authToken))
                {
                    return Json(new { success = false, message = "No se encontró el token de autenticación." }, JsonRequestBehavior.AllowGet);
                }

                updateProxy(authToken);

                // Obtener todas las categorías
                var categories = await _proxy.GetAllCategories();

                // Retornar los datos en formato JSON
                return Json(categories, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // En caso de error, devolver el error como JSON
                return Json(new { success = false, message = "Error al obtener las categorías: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }



        private void updateProxy(string token)
        {
            HttpClient client = new HttpClient { BaseAddress = new Uri("http://localhost:5123/api") }; // Asegúrate de poner la URL correcta de tu API
            client.DefaultRequestHeaders.Add("Authorization", token);
            _proxy = new Proxy(client);
        }

        [HttpPost]
        public async Task<ActionResult> CreateCategory(Categories category)
        {
            try
            {
                string authToken = Request.Headers["Authorization"];

                if (string.IsNullOrEmpty(authToken))
                {
                    return Json(new { success = false, message = "No se encontró el token de autenticación." });
                }

                updateProxy(authToken);

                var createdCategory = await _proxy.CreateCategory(category);
                if (createdCategory != null)
                {
                    return Json(new { success = true, message = "Categoría creada exitosamente", category = createdCategory });
                }

                return Json(new { success = false, message = "No se pudo crear la categoría." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult> UpdateCategory(Categories category)
        {
            try
            {
                string authToken = Request.Headers["Authorization"];

                if (string.IsNullOrEmpty(authToken))
                {
                    return Json(new { success = false, message = "No se encontró el token de autenticación." });
                }

                updateProxy(authToken);

                var updatedCategory = await _proxy.UpdateCategory(category);
                if (updatedCategory != null)
                {
                    return Json(new { success = true, message = "Categoría actualizada exitosamente", category = updatedCategory });
                }

                return Json(new { success = false, message = "No se pudo actualizar la categoría." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }


        [HttpPost]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            try
            {
                string authToken = Request.Headers["Authorization"];

                if (string.IsNullOrEmpty(authToken))
                {
                    return Json(new { success = false, message = "No se encontró el token de autenticación." });
                }

                updateProxy(authToken);

                var success = await _proxy.DeleteCategory(id);
                if (success)
                {
                    return Json(new { success = true, message = "Categoría eliminada exitosamente" });
                }

                return Json(new { success = false, message = "No se pudo eliminar la categoría." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }

    }
}
