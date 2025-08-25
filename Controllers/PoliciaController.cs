using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace SistemaCarcel.Controllers
{
    public class PoliciaController : Controller
    {
        public IActionResult Index()
        {
            var rol = HttpContext.Session.GetString("Rol");
            if (rol != "policia")
                return RedirectToAction("Login", "Cuenta");

            return View();
        }
    }
}
