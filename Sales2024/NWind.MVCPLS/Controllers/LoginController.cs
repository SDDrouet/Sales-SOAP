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

        [HttpPost]
        public async Task<ActionResult> Index(LoginRequest loginRequest)
        {
            // Usar el Proxy para obtener el token
            var token = await _proxy.LoginUserAsync(loginRequest);

            // Pasar el token a la vista usando ViewBag
            ViewBag.Token = token;

            // O bien, pasar el token como modelo
            return View("Token", (object)token);
        }
    }
}
