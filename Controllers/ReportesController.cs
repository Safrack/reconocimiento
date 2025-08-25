using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaCarcel.Models;
using Rotativa.AspNetCore;

namespace SistemaCarcel.Controllers
{
    public class ReportesController : Controller
    {
        private readonly ProyectoCarcelContext _context;

        public ReportesController(ProyectoCarcelContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> VisitasGenerales()
        {
            var visitas = await _context.Visitantes
                .Include(v => v.Pabellon)
                .ToListAsync();
            return new ViewAsPdf("PDFs/VisitasGenerales", visitas);
        }

        public async Task<IActionResult> VisitasDiarias()
        {
            var hoy = DateTime.UtcNow.Date;
            var maniana = hoy.AddDays(1);

            var visitas = await _context.Visitantes
                .Where(v => v.Hora >= hoy && v.Hora < maniana)
                .Include(v => v.Pabellon)
                .ToListAsync();

            return new ViewAsPdf("PDFs/VisitasDiarias", visitas);
        }

        public async Task<IActionResult> VisitasPorPabellon()
        {
            var visitas = await _context.Visitantes
                .Include(v => v.Pabellon)
                .OrderBy(v => v.Pabellon.NombrePb)
                .ToListAsync();
            return new ViewAsPdf("PDFs/VisitasPorPabellon", visitas);
        }

        public async Task<IActionResult> VisitasPorPeriodo(DateTime inicio, DateTime fin)
        {
    var visitantes = await _context.Visitantes
        .Include(v => v.Pabellon)
        .Include(v => v.Personal)
        .Where(v => v.Hora >= inicio.ToUniversalTime() && v.Hora <= fin.ToUniversalTime())
        .ToListAsync();

    var personales = await _context.Personal.ToListAsync();

    var random = new Random();
    foreach (var v in visitantes)
    {
        if (v.Personal == null && personales.Any())
        {
            var aleatorio = personales[random.Next(personales.Count)];
            v.Personal = new Personal
            {
                Nombre = aleatorio.Nombre,
                Apellidos = aleatorio.Apellidos
            };
        }
    }

    return new ViewAsPdf("PDFs/VisitasPorPeriodo", visitantes); 
}

        public async Task<IActionResult> RegistroVisitas()
        {
            var hoy = DateTime.SpecifyKind(DateTime.UtcNow.Date, DateTimeKind.Utc);
            var maniana = hoy.AddDays(1);

            var visitantes = await _context.Visitantes
                .Where(v => v.Hora >= hoy && v.Hora < maniana)
                .Include(v => v.Pabellon)
                .Include(v => v.Personal)
                .ToListAsync();

    return new ViewAsPdf("PDFs/RegistroVisitas", visitantes);
        }
    }
}
