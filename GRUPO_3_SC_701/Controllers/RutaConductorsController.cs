using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GRUPO_3_SC_701.Data;
using GRUPO_3_SC_701.Models;
using Microsoft.AspNetCore.Authorization;

namespace GRUPO_3_SC_701.Controllers
{
  
    public class RutaConductorsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RutaConductorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: RutaConductors
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.RutaConductores.Include(r => r.Ruta).Include(r => r.Vehiculo).ThenInclude(u=>u.UsuarioRegistro);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: RutaConductors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rutaConductor = await _context.RutaConductores
                .Include(r => r.Ruta)
                .Include(r => r.Vehiculo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rutaConductor == null)
            {
                return NotFound();
            }

            return View(rutaConductor);
        }

        // GET: RutaConductors/Create
        public IActionResult Create()
        {
            ViewData["RutaId"] = new SelectList(_context.Rutas, "Id", "Nombre");
            ViewData["VehiculoId"] = new SelectList(_context.Vehiculos, "Id", "Modelo");
            return View();
        }

        // POST: RutaConductors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RutaId,VehiculoId")] RutaConductor rutaConductor)
        {
            if (!ModelState.IsValid)
            {
                _context.Add(rutaConductor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RutaId"] = new SelectList(_context.Rutas, "Id", "Nombre", rutaConductor.RutaId);
            ViewData["VehiculoId"] = new SelectList(_context.Vehiculos, "Id", "Modelo", rutaConductor.VehiculoId);
            return View(rutaConductor);
        }

        // GET: RutaConductors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rutaConductor = await _context.RutaConductores.FindAsync(id);
            if (rutaConductor == null)
            {
                return NotFound();
            }
            ViewData["RutaId"] = new SelectList(_context.Rutas, "Id", "Nombre", rutaConductor.RutaId);
            ViewData["VehiculoId"] = new SelectList(_context.Vehiculos, "Id", "Modelo", rutaConductor.VehiculoId);
            return View(rutaConductor);
        }

        // POST: RutaConductors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RutaId,VehiculoId")] RutaConductor rutaConductor)
        {
            if (id != rutaConductor.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(rutaConductor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RutaConductorExists(rutaConductor.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["RutaId"] = new SelectList(_context.Rutas, "Id", "Nombre", rutaConductor.RutaId);
            ViewData["VehiculoId"] = new SelectList(_context.Vehiculos, "Id", "Modelo", rutaConductor.VehiculoId);
            return View(rutaConductor);
        }

        // GET: RutaConductors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rutaConductor = await _context.RutaConductores
                .Include(r => r.Ruta)
                .Include(r => r.Vehiculo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rutaConductor == null)
            {
                return NotFound();
            }

            return View(rutaConductor);
        }

        // POST: RutaConductors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rutaConductor = await _context.RutaConductores.FindAsync(id);
            if (rutaConductor != null)
            {
                _context.RutaConductores.Remove(rutaConductor);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RutaConductorExists(int id)
        {
            return _context.RutaConductores.Any(e => e.Id == id);
        }
    }
}
