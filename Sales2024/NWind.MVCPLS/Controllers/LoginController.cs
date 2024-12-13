using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using proxy;
using Entities;
using System.Threading.Tasks;

namespace NWind.MVCPLS.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Index(LoginRequest loginRequest)
        {
            var proxy = new Proxy();
            var token = await proxy.Login(loginRequest); // Asegúrate de que `Login` sea async y retorna el token.

            // Pasar el token a la vista usando ViewBag
            ViewBag.Token = token;

            // O bien, pasar el token como modelo
            return View("Token", (object)token);
        }
    }
}