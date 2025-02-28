using GRUPO_3_SC_701.Data;
using GRUPO_3_SC_701.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GRUPO_3_SC_701.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoutesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RoutesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/routes
        [HttpGet("list")]
        public async Task<IActionResult> GetRoutes()
        {
            var routes = await _context.Rutas
                .Where(r => r.Estado)
                .Select(r => new
                {
                    r.Id,
                    r.Codigo,
                    r.Nombre,
                    r.Descripcion,
                    Estado = r.Estado,
                    Horarios = r.Horarios.Select(h => new { h.Id, h.Hora })
                })
                .ToListAsync();

            return Ok(routes);
        }

        // GET: api/routes/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetRoute(int id)
        {
            var route = await _context.Rutas
                .Include(r => r.Paradas)
                .Include(r => r.Horarios)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (route == null)
            {
                return NotFound(new { Message = "Ruta no encontrada." });
            }

            var result = new
            {
                route.Id,
                route.Codigo,
                route.Nombre,
                route.Descripcion,
                Estado = route.Estado,
                Paradas = route.Paradas.Select(p => new { p.Id, p.Nombre, p.Orden }),
                Horarios = route.Horarios.Select(h => new { h.Id, h.Hora, h.VehiculoId })
            };

            return Ok(result);
        }
    }
}
