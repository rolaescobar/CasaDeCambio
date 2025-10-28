using CasaDeCambio.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CasaDeCambio.Controllers
{
    public class PersonasController : Controller
    {
        private readonly ProyectoFinalDbContext _context;

        public PersonasController(ProyectoFinalDbContext context)
        {

            _context = context;
        }

        public async Task<IActionResult> Index(string buscar)
        {
            var query = _context.Personas.AsQueryable();
            
            if(!string.IsNullOrWhiteSpace(buscar))
            {
                buscar = buscar.Trim();
                query = query.Where(p => p.Nombre.Contains(buscar) || p.DPI.ToString().Contains(buscar));

            }

            var lista = await query.OrderBy(p => p.Nombre).ToListAsync();

            return View(lista);
        }


    
    }
}
