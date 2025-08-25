using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaCarcel.Models;

namespace SistemaCarcel.Controllers
{
    public class ReclusosController : Controller
    {
        private readonly ProyectoCarcelContext _context;

        public ReclusosController(ProyectoCarcelContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var reclusos = await _context.Reclusos
                .Include(r => r.Pabellon)
                .OrderBy(r => r.IdRecluso) // Mantener orden fijo
                .ToListAsync();

            ViewBag.Pabellones = await _context.Pabellones.ToListAsync();
            return View(reclusos);
        }

        [HttpGet]
        public async Task<IActionResult> GetRecluso(int id)
        {
            var recluso = await _context.Reclusos
                .Include(r => r.Pabellon)
                .FirstOrDefaultAsync(r => r.IdRecluso == id);

            if (recluso == null)
                return NotFound();

            return Json(new
            {
                recluso.IdRecluso,
                recluso.NombreCompleto,
                recluso.Ci,
                recluso.Edad,
                recluso.Delito,
                FechaIngreso = recluso.FechaIngreso.ToString("yyyy-MM-dd"),
                recluso.IdPabellon
            });
        }

        [HttpPost]
    public async Task<IActionResult> Create([FromForm] Recluso recluso)
    {
         if (!ModelState.IsValid)
    {
        var errores = ModelState
            .Select(kvp => new { Campo = kvp.Key, Errores = kvp.Value.Errors.Select(e => e.ErrorMessage) })
            .ToList();

        return BadRequest(new { errores });
    }

    recluso.FechaIngreso = DateTime.SpecifyKind(recluso.FechaIngreso, DateTimeKind.Utc);
    _context.Reclusos.Add(recluso);
    await _context.SaveChangesAsync();
    return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> Update([FromForm] Recluso recluso)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new
            {
                errores = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()
            });
        }

        recluso.FechaIngreso = DateTime.SpecifyKind(recluso.FechaIngreso, DateTimeKind.Utc);

        _context.Reclusos.Update(recluso);
        await _context.SaveChangesAsync();
        return Ok();
    }

    }
}
