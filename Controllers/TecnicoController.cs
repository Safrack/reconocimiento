using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace SistemaCarcel.Controllers
{
    public class TecnicoController : Controller
    {
        public IActionResult Index()
        {
            var rol = HttpContext.Session.GetString("Rol");
            if (rol != "tecnico")
                return RedirectToAction("Login", "Cuenta");

            return View();
        }
    }
}
