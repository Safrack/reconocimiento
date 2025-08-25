using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaCarcel.Models;

namespace SistemaCarcel.Controllers
{
    public class PersonalController : Controller
    {
        private readonly ProyectoCarcelContext _context;

        public PersonalController(ProyectoCarcelContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var personal = await _context.Personal
                .Include(p => p.Users)
                .ToListAsync();

            return View(personal);
        }

        // Obtener datos para editar (AJAX)
        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            var persona = await _context.Personal.FindAsync(id);
            if (persona == null) return NotFound();

            return Json(new
            {
                idPersonal = persona.IdPersonal,
                nombre = persona.Nombre,
                apellidos = persona.Apellidos,
                ci = persona.Ci,
                sexo = persona.Sexo,
                cargo = persona.Cargo
            });
        }

        // Crear nuevo personal (AJAX)
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] Personal personal)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (personal.FechaNacimiento.HasValue)
            personal.FechaNacimiento = DateTime.SpecifyKind(personal.FechaNacimiento.Value, DateTimeKind.Utc);

            _context.Personal.Add(personal);
            await _context.SaveChangesAsync();

            return Json(new
            {
                idPersonal = personal.IdPersonal,
                personal.Nombre,
                personal.Apellidos,
                personal.Ci,
                personal.Sexo,
                personal.Cargo
            });
        }

        // Actualizar personal (AJAX)
        [HttpPost]
public async Task<IActionResult> Update([FromForm] Personal personal)
{
    var existing = await _context.Personal.FindAsync(personal.IdPersonal);
    if (existing == null)
        return NotFound();

    existing.Nombre = personal.Nombre;
    existing.Apellidos = personal.Apellidos;
    existing.Ci = personal.Ci;
    existing.Sexo = personal.Sexo;
    existing.Cargo = personal.Cargo;

    if (personal.FechaNacimiento.HasValue)
        existing.FechaNacimiento = DateTime.SpecifyKind(personal.FechaNacimiento.Value, DateTimeKind.Utc);

    await _context.SaveChangesAsync();

    return Json(new
    {
        idPersonal = existing.IdPersonal,
        existing.Nombre,
        existing.Apellidos,
        existing.Ci,
        existing.Sexo,
        existing.Cargo
    });
}

        public async Task<IActionResult> Delete(int id)
        {
            var persona = await _context.Personal.FindAsync(id);
            if (persona != null)
            {
                _context.Personal.Remove(persona);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}