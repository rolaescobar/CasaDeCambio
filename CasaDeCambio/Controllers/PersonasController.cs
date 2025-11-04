using CasaDeCambio.Data;
using CasaDeCambio.Models;
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

        // GET: Personas
        public async Task<IActionResult> Index(string buscar)
        {
            var query = _context.Personas
                .Include(p => p.CambiosDivisas)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(buscar))
            {
                buscar = buscar.Trim();
                query = query.Where(p =>
                    p.Nombre.Contains(buscar) ||
                    p.DPI.ToString().Contains(buscar));
            }

            var lista = await query
                .OrderBy(p => p.Nombre)
                .ToListAsync();

            return View(lista);
        }

        // GET: Personas/Create
        public IActionResult Create() => View();

        // POST: Personas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPersona,Nombre,DPI")] Persona persona)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(persona);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", $"No se pudo guardar. ¿DPI duplicado? Detalle: {ex.Message}");
                }
            }
            return View(persona);
        }

        // GET: Personas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var persona = await _context.Personas.FindAsync(id);
            if (persona == null) return NotFound();

            return View(persona);
        }

        // POST: Personas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdPersona,Nombre,DPI")] Persona persona)
        {
            if (id != persona.IdPersona) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(persona);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Personas.Any(e => e.IdPersona == id))
                        return NotFound();
                    throw;
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", $"No se pudo actualizar. ¿DPI duplicado? Detalle: {ex.Message}");
                }
            }
            return View(persona);
        }

        // GET: Personas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var persona = await _context.Personas
                .FirstOrDefaultAsync(m => m.IdPersona == id);

            if (persona == null) return NotFound();

            return View(persona);
        }

        // POST: Personas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var persona = await _context.Personas.FindAsync(id);
            if (persona != null)
            {
                _context.Personas.Remove(persona);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Personas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var persona = await _context.Personas
                .FirstOrDefaultAsync(p => p.IdPersona == id);

            if (persona == null) return NotFound();

            return View(persona);
        }

        // =========================
        //  CAMBIOS DE DIVISA
        // =========================

        // GET: Personas/AddCambio/5
        // acepta ?idPersona=5 o /AddCambio/5
        public IActionResult AddCambio(int idPersona, int id)
        {
            // soporte para las dos formas
            var personaId = idPersona != 0 ? idPersona : id;

            if (personaId == 0)
            {
                // si no vino id, mejor lo mandamos al listado
                return RedirectToAction(nameof(Index));
            }

            var modelo = new CambiosDivisa
            {
                IdPersona = personaId
            };

            return View(modelo);
        }

        // POST: Personas/AddCambio
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCambio(CambiosDivisa cambio)
        {
            // esta propiedad no viene del form, la ponemos nosotros
            ModelState.Remove(nameof(CambiosDivisa.FechaHoraCambio));

            // validar que la persona exista
            var personaExiste = await _context.Personas
                .AnyAsync(p => p.IdPersona == cambio.IdPersona);

            if (!personaExiste)
            {
                ModelState.AddModelError("", "La persona seleccionada no existe.");
            }

            
            cambio.FechaHoraCambio = DateTime.Now;

            _context.CambiosDivisas.Add(cambio);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
