using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace SistemaCarcel.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            var rol = HttpContext.Session.GetString("Rol");
            if (rol != "administrador")
                return RedirectToAction("Login", "Cuenta");

            return View();
        }
    }
}
