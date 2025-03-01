using System.Linq;
using System.Threading.Tasks;
using GRUPO_3_SC_701.Data;
using GRUPO_3_SC_701.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GRUPO_3_SC_701.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class RutasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public RutasController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: /Rutas
        public async Task<IActionResult> Index()
        {
            var rutas = await _context.Rutas.Include(r => r.Horarios).ToListAsync();
            return View(rutas);
        }

        // GET: /Rutas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Rutas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Ruta ruta)
        {
            if (ModelState.IsValid)
            {
                var usuarioActual = await _userManager.GetUserAsync(User);
                if (usuarioActual == null) return Unauthorized();

                ruta.FechaRegistro = DateTime.Now;
                ruta.UsuarioRegistroId = usuarioActual.Id;

                _context.Rutas.Add(ruta);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ruta);
        }

        // GET: /Rutas/Edit/{id}
        public async Task<IActionResult> Edit(int id)
        {
            var ruta = await _context.Rutas.FindAsync(id);
            if (ruta == null)
            {
                return NotFound();
            }
            return View(ruta);
        }

        // POST: /Rutas/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Ruta ruta)
        {
            if (id != ruta.Id)
            {
                return NotFound();
            }
           
            if (ModelState.IsValid)
            {
                var usuarioActual = await _userManager.GetUserAsync(User);
                if (usuarioActual == null) return Unauthorized();

                ruta.UsuarioRegistroId = usuarioActual.Id;
                _context.Update(ruta);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ruta);
        }

        // GET: /Rutas/Delete/{id}
        public async Task<IActionResult> Delete(int id)
        {
            var ruta = await _context.Rutas.FindAsync(id);
            if (ruta == null)
            {
                return NotFound();
            }
            return View(ruta);
        }

        // POST: /Rutas/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ruta = await _context.Rutas.FindAsync(id);
            if (ruta != null)
            {
                _context.Rutas.Remove(ruta);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
