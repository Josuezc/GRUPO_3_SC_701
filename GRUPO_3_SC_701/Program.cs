using GRUPO_3_SC_701.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using GRUPO_3_SC_701.Models;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("GRUPO_3_SC_701") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

#region Minimal API Endpoints

app.MapGet("/api/routes", async (ApplicationDbContext db) =>
{
    var routes = await db.Rutas
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
    return Results.Ok(routes);
});

app.MapGet("/api/routes/{id:int}", async (int id, ApplicationDbContext db) =>
{
    var route = await db.Rutas
        .Include(r => r.Paradas)
        .Include(r => r.Horarios)
        .FirstOrDefaultAsync(r => r.Id == id);

    if (route == null)
    {
        return Results.NotFound(new { Message = "Ruta no encontrada." });
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

    return Results.Ok(result);
});

app.MapGet("/dashboard/user-tickets", async (ApplicationDbContext db) =>
{
    var userTickets = await db.Boletos
        .Include(b => b.Usuario)
        .Include(b => b.Horario)
            .ThenInclude(h => h.Ruta)
        .GroupBy(b => new { b.UsuarioId, b.Usuario.UserName })
        .Select(g => new
        {
            UserId = g.Key.UsuarioId,
            UserName = g.Key.UserName,
            Tickets = g.Select(b => new
            {
                TicketId = b.Id,
                RouteName = b.Horario.Ruta.Nombre,
                Schedule = b.Horario.Hora,
                DatePurchased = b.FechaCompra
            }).ToList()
        })
        .ToListAsync();

    return Results.Ok(userTickets);
});

app.MapGet("/dashboard/summary", async (ApplicationDbContext db) =>
{
    var totalActiveRoutes = await db.Rutas.CountAsync(r => r.Estado);
    var totalGoodVehicles = await db.Vehiculos.CountAsync(v => v.Estado == EstadoVehiculo.Bueno);
    var now = DateTime.Now;
    var totalTicketsCurrentMonth = await db.Boletos.CountAsync(b => b.FechaCompra.Month == now.Month && b.FechaCompra.Year == now.Year);

    var summary = new
    {
        TotalActiveRoutes = totalActiveRoutes,
        TotalGoodVehicles = totalGoodVehicles,
        TotalTicketsCurrentMonth = totalTicketsCurrentMonth
    };

    return Results.Ok(summary);
});

#endregion

app.Run();
