using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Entities;
using proxy;

namespace NWind.MVCPLS.Controllers
{
    public class ProductController : Controller
    {
        private Proxy _proxy;

        // Vista para gestionar todos los productos
        public ActionResult ManageProducts()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> GetAllProducts()
        {
            try
            {
                // Recuperar el token de la sesión (del encabezado Authorization)
                string authToken = Request.Headers["Authorization"];

                if (string.IsNullOrEmpty(authToken))
                {
                    return Json(new { success = false, message = "No se encontró el token de autenticación." }, JsonRequestBehavior.AllowGet);
                }

                UpdateProxy(authToken);

                // Obtener todos los productos
                var products = await _proxy.GetAllProducts();

                // Retornar los datos en formato JSON
                return Json(products, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // En caso de error, devolver el error como JSON
                return Json(new { success = false, message = "Error al obtener los productos: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        private void UpdateProxy(string token)
        {
            HttpClient client = new HttpClient { BaseAddress = new Uri("http://localhost:5123/api") }; // Cambia la URL base según corresponda
            client.DefaultRequestHeaders.Add("Authorization", token);
            _proxy = new Proxy(client);
        }

        [HttpPost]
        public async Task<ActionResult> CreateProduct(Products product)
        {
            try
            {
                string authToken = Request.Headers["Authorization"];

                if (string.IsNullOrEmpty(authToken))
                {
                    return Json(new { success = false, message = "No se encontró el token de autenticación." });
                }

                UpdateProxy(authToken);

                var createdProduct = await _proxy.CreateProduct(product);
                if (createdProduct != null)
                {
                    return Json(new { success = true, message = "Producto creado exitosamente", product = createdProduct });
                }

                return Json(new { success = false, message = "No se pudo crear el producto." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult> UpdateProduct(Products product)
        {
            try
            {
                string authToken = Request.Headers["Authorization"];

                if (string.IsNullOrEmpty(authToken))
                {
                    return Json(new { success = false, message = "No se encontró el token de autenticación." });
                }

                UpdateProxy(authToken);

                var success = await _proxy.UpdateProduct(product);
                if (success)
                {
                    return Json(new { success = true, message = "Producto actualizado exitosamente" });
                }

                return Json(new { success = false, message = "No se pudo actualizar el producto." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            try
            {
                string authToken = Request.Headers["Authorization"];

                if (string.IsNullOrEmpty(authToken))
                {
                    return Json(new { success = false, message = "No se encontró el token de autenticación." });
                }

                UpdateProxy(authToken);

                var success = await _proxy.DeleteProduct(id);
                if (success)
                {
                    return Json(new { success = true, message = "Producto eliminado exitosamente" });
                }

                return Json(new { success = false, message = "No se pudo eliminar el producto." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }
    }
}