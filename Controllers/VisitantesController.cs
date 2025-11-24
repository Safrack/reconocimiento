using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaCarcel.Models;
using Rotativa.AspNetCore;
using System.Text.Json;

namespace SistemaCarcel.Controllers
{
    public class VisitantesController : Controller
    {
        private readonly ProyectoCarcelContext _context;

        public VisitantesController(ProyectoCarcelContext context)
        {
            _context = context;
        }

        // GET: Visitantes
        public async Task<IActionResult> Index()
        {
            var visitas = await _context.Visitantes
                .Include(v => v.Pabellon)
                .ToListAsync();

            ViewBag.PabellonId = new SelectList(_context.Pabellones, "Id", "NombrePb");
            return View(visitas);
        }

        // POST: Visitantes/GenerarReporte
       [HttpPost]
        public IActionResult GenerarReporte([FromBody] List<VisitanteReporteViewModel> visitantes)
        {
            if (visitantes == null || visitantes.Count == 0)
                return BadRequest("No se enviaron datos para el reporte.");

            return new ViewAsPdf("ReportePdf", visitantes)
            {
                FileName = "Reporte_Visitantes.pdf",
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait
            };
        }
    }

    // ViewModel para el reporte
    public class VisitanteReporteViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Pabellon { get; set; } = null!;
        public int Edad { get; set; }
        public string Recluso { get; set; } = null!;
        public string Fecha { get; set; } = null!;
    }
}
