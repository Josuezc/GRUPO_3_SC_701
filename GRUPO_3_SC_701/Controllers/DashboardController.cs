using GRUPO_3_SC_701.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GRUPO_3_SC_701.ViewModels;
using GRUPO_3_SC_701.Models;

namespace GRUPO_3_SC_701.Controllers
{
    public class DashboardController : Controller
    {

        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index()
        {

            var userTickets = await _context.Boletos
                .Include(b => b.Usuario)
                .Include(b => b.Horario)
                    .ThenInclude(h => h.Ruta)
                .Select(b => new UserTicketInfo
                {
                    Usuario = b.Usuario.UserName,
                    Ruta = b.Horario.Ruta.Nombre,
                    Horario = b.Horario.Hora,
                    FechaCompra = b.FechaCompra
                })
                .ToListAsync();


            var totalActiveRoutes = await _context.Rutas.CountAsync(r => r.Estado);

            var totalGoodVehicles = await _context.Vehiculos
                .CountAsync(v => v.Estado == EstadoVehiculo.Bueno);

            var totalRegularVehicles = await _context.Vehiculos
                .CountAsync(v => v.Estado == EstadoVehiculo.Regular);

            var totalMaintenanceVehicles = await _context.Vehiculos
                .CountAsync(v => v.Estado == EstadoVehiculo.NecesitaMantenimiento);

            var now = DateTime.Now;
            var totalTicketsCurrentMonth = await _context.Boletos.CountAsync(b => b.FechaCompra.Month == now.Month && b.FechaCompra.Year == now.Year);

            var dashboardViewModel = new DashboardViewModel
            {
                UserTickets = userTickets,
                TotalActiveRoutes = totalActiveRoutes,
                TotalGoodVehicles = totalGoodVehicles,
                TotalRegularVehicles = totalRegularVehicles,
                TotalMaintenanceVehicles = totalMaintenanceVehicles,
                TotalTicketsCurrentMonth = totalTicketsCurrentMonth
            };

            return View(dashboardViewModel);
        }
    }
}
