using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaCarcel.Models;

namespace SistemaCarcel.Controllers
{
    public class VisitantesController : Controller
    {
        private readonly ProyectoCarcelContext _context;

        public VisitantesController(ProyectoCarcelContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var visitantes = await _context.Visitantes
                .Include(v => v.Pabellon)
                .ToListAsync();

            ViewBag.PabellonId = new SelectList(_context.Pabellones, "Id", "NombrePb");
            ViewBag.Reclusos = await _context.Reclusos.ToListAsync();

            return View(visitantes);
        }

        public IActionResult Create()
        {
            ViewBag.PabellonId = new SelectList(_context.Pabellones, "Id", "Nombre");
            return View();
        }

        [HttpPost]
public async Task<IActionResult> Create([FromForm] Visitante visitante, [FromForm] string Imagen)
{
    try
    {
        visitante.Hora = DateTime.UtcNow;
        visitante.Estado = true;

        // Guardar la imagen
        if (!string.IsNullOrEmpty(Imagen))
        {
            var base64 = Imagen.Substring(Imagen.IndexOf(',') + 1);
            var bytes = Convert.FromBase64String(base64);
            var nombreArchivo = $"{Guid.NewGuid()}.jpg";
            var rutaRelativa = $"/img/visitantes/{nombreArchivo}";
            var rutaGuardado = Path.Combine("wwwroot", "img", "visitantes", nombreArchivo);

            await System.IO.File.WriteAllBytesAsync(rutaGuardado, bytes);

            visitante.Imagen = rutaRelativa;

            // Copiar a known_faces con formato: Nombre_CI.jpg
            var nombreFormateado = $"{visitante.NombreCompleto.Trim()}_{visitante.Ci}";
            var rutaDestino = Path.Combine("ReconocimientoFacial", "known_faces", $"{nombreFormateado}.jpg");

            System.IO.File.Copy(rutaGuardado, rutaDestino, overwrite: true);
        }

        ModelState.Remove("Pabellon");
        ModelState.Remove("Personal");
        ModelState.Remove("Autorizacion");
        if (visitante.Edad < 18 || visitante.Edad > 99)
        {
            return BadRequest(new { error = "Solo se permiten visitantes mayores de edad (18 a 99 años)." });
        }

        if (!ModelState.IsValid)
        {
            var errores = ModelState
                .Where(x => x.Value.Errors.Count > 0)
                .ToDictionary(
                    x => x.Key,
                    x => x.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                );

            return BadRequest(new { errors = errores });
        }

        _context.Visitantes.Add(visitante);
        await _context.SaveChangesAsync();

        return Json(new { success = true });
    }
    catch (Exception ex)
    {
        return StatusCode(500, new { error = ex.Message, inner = ex.InnerException?.Message });
    }
}

    }
}