using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using SistemaCarcel.Models;

namespace SistemaCarcel.Controllers
{
    public class CuentaController : Controller
    {
        private readonly ProyectoCarcelContext _context;

        public CuentaController(ProyectoCarcelContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

       [HttpPost]
public IActionResult Login(string username, string password)
{
    var user = _context.Users.FirstOrDefault(u => u.Username == username);

if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
{
    ViewBag.Error = "Credenciales incorrectas";
    return View();
}


    if (!user.Estado)
    {
        ViewBag.Error = "El usuario se encuentra deshabilitado.";
        return View();
    }

    // Guardar el rol y el nombre de usuario en sesión
    HttpContext.Session.SetString("Rol", user.Rol);
    HttpContext.Session.SetString("Username", user.Username);

    return RedirectToAction("Index", user.Rol switch
    {
        "administrador" => "Admin",
        "tecnico" => "Tecnico",
        "policia" => "Policia",
        _ => "Cuenta"
    });
}

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
