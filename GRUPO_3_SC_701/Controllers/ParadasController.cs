using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GRUPO_3_SC_701.Data;
using GRUPO_3_SC_701.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GRUPO_3_SC_701.Controllers
{
    public class ParadasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ParadasController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var paradas = await _context.Paradas.Include(p => p.Ruta).ToListAsync();
            return View(paradas);
        }

        public IActionResult Create()
        {
            ViewBag.Rutas = new SelectList(_context.Rutas, "Id", "Nombre");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Parada parada)
        {
            if (parada.RutaId == 0)
            {
                ModelState.AddModelError("RutaId", "Debe seleccionar una ruta válida.");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Rutas = new SelectList(_context.Rutas, "Id", "Nombre");
                return View(parada);
            }

            var ruta = await _context.Rutas.FindAsync(parada.RutaId);
            if (ruta == null)
            {
                ModelState.AddModelError("RutaId", "La ruta seleccionada no es válida.");
                ViewBag.Rutas = new SelectList(_context.Rutas, "Id", "Nombre");
                return View(parada);
            }

            _context.Paradas.Add(parada);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var parada = await _context.Paradas.FindAsync(id);
            if (parada == null)
            {
                return NotFound();
            }

            ViewBag.Rutas = new SelectList(_context.Rutas, "Id", "Nombre", parada.RutaId);
            return View(parada);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Parada parada)
        {
            if (id != parada.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(parada);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Rutas = new SelectList(_context.Rutas, "Id", "Nombre", parada.RutaId);
            return View(parada);
        }


        public async Task<IActionResult> Delete(int id)
        {
            var parada = await _context.Paradas
                .Include(p => p.Ruta) 
                .FirstOrDefaultAsync(p => p.Id == id);

            if (parada == null)
            {
                return NotFound();
            }
            return View(parada);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var parada = await _context.Paradas.FindAsync(id);
            if (parada != null)
            {
                _context.Paradas.Remove(parada);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
