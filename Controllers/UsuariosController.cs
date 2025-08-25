using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using SistemaCarcel.Models;

namespace SistemaCarcel.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly ProyectoCarcelContext _context;

        public UsuariosController(ProyectoCarcelContext context)
        {
            _context = context;
        }

        // Listado de usuarios
        public async Task<IActionResult> Index()
        {
            var usuarios = await _context.Users.Include(u => u.Personal).ToListAsync();
            return View(usuarios);
        }

        // Obtener usuario (AJAX)
        [HttpGet]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            return Json(new
            {
                user.Id,
                user.Username,
                user.Rol,
                user.Estado
            });
        }

        // Crear nuevo usuario desde modal (por AJAX)
        [HttpPost]
        public async Task<IActionResult> Guardar([FromForm] User user)
        {
             if (!ModelState.IsValid)
    {
        var errores = ModelState.Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage)
            .ToList();

        return BadRequest("Errores de modelo: " + string.Join(", ", errores));
    }

    // VERIFICAR SI YA EXISTE UN USUARIO PARA ESE PERSONAL
    var yaExiste = await _context.Users.AnyAsync(u => u.IdPersonal == user.IdPersonal);
    if (yaExiste)
    {
        return BadRequest("Este personal ya tiene un usuario asignado.");
    }

    user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.Password); 
    user.CreatedAt = DateTime.UtcNow;

    _context.Users.Add(user);
    await _context.SaveChangesAsync();

    return Json(new
    {
        success = true,
        message = "Usuario creado correctamente"
    });
        }

        // Actualizar usuario (AJAX)
        [HttpPost]
        public async Task<IActionResult> UpdateUser([FromForm] User user)
        {
            var existing = await _context.Users.FindAsync(user.Id);
            if (existing == null)
                return NotFound();

            existing.Username = user.Username;
            existing.Rol = user.Rol;
            existing.Estado = user.Estado;

            if (!string.IsNullOrWhiteSpace(user.Password))
            {
        // Solo si se ingresó una nueva contraseña, la actualizamos
        existing.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.Password);
            }

            await _context.SaveChangesAsync();

            return Json(new
            {
                success = true,
                message = "Usuario actualizado correctamente"
            });
        }

        // Habilitar/Inhabilitar usuario
        public async Task<IActionResult> ToggleEstado(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            user.Estado = !user.Estado;
            user.CreatedAt = DateTime.SpecifyKind(user.CreatedAt, DateTimeKind.Utc);
            _context.Update(user);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult GetPorPersonal(int id)
        {
            var usuario = _context.Users.FirstOrDefault(u => u.IdPersonal == id);
            if (usuario == null)
                return NotFound();

            return Json(new
            {
                id = usuario.Id,
                username = usuario.Username,
                rol = usuario.Rol,
                estado = usuario.Estado
            });
        }
    }
}