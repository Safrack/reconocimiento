using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaCarcel.Models;

namespace SistemaCarcel.Controllers
{
    public class AsignacionPabellonPolController : Controller
    {
        private readonly ProyectoCarcelContext _context;

        public AsignacionPabellonPolController(ProyectoCarcelContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var asignaciones = await _context.Asignaciones
                .Include(a => a.Pabellon)
                .Include(a => a.Personal)
                .ToListAsync();
            return View(asignaciones);
        }
    }
}
