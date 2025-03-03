using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GRUPO_3_SC_701.Data;
using GRUPO_3_SC_701.Models;
using System.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace GRUPO_3_SC_701.Controllers
{
    public class BoletoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BoletoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Boletoes

        public async Task<IActionResult> Index()
        {
            //obtiene el id del usuario logeado
            string userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var userRole = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
            if (userRole=="Usuario")
            {

                var boletoUsuario = _context.Boletos.Include(b => b.Horario).ThenInclude(b => b.Ruta).Include(b => b.Usuario).Where(b=>b.UsuarioId == userId);
                return View(await boletoUsuario.ToListAsync());
            }
            if (userRole == "Conductor")
            {
                var boletos = await _context.Boletos
                    .Include(b => b.Horario)
                        .ThenInclude(h => h.Ruta)
                            .ThenInclude(r => r.RutaConductores)
                                .ThenInclude(rc => rc.Vehiculo)
                                    .ThenInclude(v => v.UsuarioRegistro)
                    .Where(b => b.Horario.Ruta.RutaConductores
                        .Any(rc => rc.Vehiculo.UsuarioRegistroId == userId)) 
                    .ToListAsync();

                return View(boletos);
            }
            //admin
            var applicationDbContext = _context.Boletos.Include(b => b.Horario).ThenInclude(b => b.Ruta).Include(b => b.Usuario);
            return View(await applicationDbContext.ToListAsync());

        }

        // GET: Boletoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var boleto = await _context.Boletos
                .Include(b => b.Horario)
                .Include(b => b.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (boleto == null)
            {
                return NotFound();
            }

            return View(boleto);
        }

        // GET: Boletoes/Create
       
        public IActionResult Create()
        {
            ViewData["HorarioId"] = new SelectList(_context.Horarios, "Id", "Hora");
            ViewData["UsuarioId"] = new SelectList(_context.Users, "Id", "UserName");
            return View();
        }

        // POST: Boletoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UsuarioId,HorarioId,FechaCompra")] Boleto boleto)
        {
            
            if (!ModelState.IsValid)
            {
                boleto.FechaCompra = DateTime.Now;
                var horario = await _context.Horarios
                    .Include(h => h.Vehiculo)
                    .FirstOrDefaultAsync(h => h.Id == boleto.HorarioId);

                if (horario == null)
                {
                    ModelState.AddModelError("", "El horario seleccionado no existe.");
                    ViewData["HorarioId"] = new SelectList(_context.Horarios, "Id", "Hora", boleto.HorarioId);
                    ViewData["UsuarioId"] = new SelectList(_context.Users, "Id", "UserName", boleto.UsuarioId);
                    return View(boleto);
                }

                // Contar los boletos ya vendidos para este horario
                int boletosVendidos = await _context.Boletos
                    .Where(b => b.HorarioId == boleto.HorarioId)
                    .CountAsync();

                if (boletosVendidos >= horario.Vehiculo.Capacidad)
                {
                    ModelState.AddModelError("", "No hay asientos disponibles en este vehículo.");
                    ViewData["HorarioId"] = new SelectList(_context.Horarios, "Id", "Hora", boleto.HorarioId);
                    ViewData["UsuarioId"] = new SelectList(_context.Users, "Id", "UserName", boleto.UsuarioId);
                    return View(boleto);
                }
                _context.Add(boleto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
          
            
            ViewData["HorarioId"] = new SelectList(_context.Horarios, "Id", "Hora", boleto.HorarioId);
            ViewData["UsuarioId"] = new SelectList(_context.Users, "Id", "UserName", boleto.UsuarioId);
            return View(boleto);
        }

        // GET: Boletoes/Edit/5
       
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var boleto = await _context.Boletos.FindAsync(id);
            if (boleto == null)
            {
                return NotFound();
            }
            ViewData["HorarioId"] = new SelectList(_context.Horarios, "Id", "Hora", boleto.HorarioId);
            ViewData["UsuarioId"] = new SelectList(_context.Users, "Id", "UserName", boleto.UsuarioId);
            return View(boleto);
        }

        // POST: Boletoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UsuarioId,HorarioId,FechaCompra")] Boleto boleto)
        {
            if (id != boleto.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(boleto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BoletoExists(boleto.Id))
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
            ViewData["HorarioId"] = new SelectList(_context.Horarios, "Id", "Hora", boleto.HorarioId);
            ViewData["UsuarioId"] = new SelectList(_context.Users, "Id", "UserName", boleto.UsuarioId);
            return View(boleto);
        }

        // GET: Boletoes/Delete/5
       
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var boleto = await _context.Boletos
                .Include(b => b.Horario)
                .Include(b => b.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (boleto == null)
            {
                return NotFound();
            }

            return View(boleto);
        }

        // POST: Boletoes/Delete/5
      
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var boleto = await _context.Boletos.FindAsync(id);
            if (boleto != null)
            {
                _context.Boletos.Remove(boleto);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BoletoExists(int id)
        {
            return _context.Boletos.Any(e => e.Id == id);
        }
    }
}
