using GRUPO_3_SC_701.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GRUPO_3_SC_701.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Ruta> Rutas { get; set; }
        public DbSet<Parada> Paradas { get; set; }
        public DbSet<Vehiculo> Vehiculos { get; set; }
        public DbSet<Horario> Horarios { get; set; }
        public DbSet<Boleto> Boletos { get; set; }
        public DbSet<RutaConductor> RutaConductores { get; set; }



        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<RutaConductor>()
                .HasOne(rc => rc.Ruta)
                .WithMany(r => r.RutaConductores)
                .HasForeignKey(rc => rc.RutaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<RutaConductor>()
                .HasOne(rc => rc.Usuario)
                .WithMany()
                .HasForeignKey(rc => rc.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Horario>()
                .HasOne(rc => rc.Ruta)
                .WithMany(r => r.Horarios)
                .HasForeignKey(rc => rc.RutaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Horario>()
                .HasOne(rc => rc.Vehiculo)
                .WithMany(r => r.Horarios)
                .HasForeignKey(rc => rc.VehiculoId)
                .OnDelete(DeleteBehavior.Restrict);


            var adminUser = new IdentityUser
            {
                UserName = "admin",
                Email = "admin@domain.com",
            };

            var hasher = new PasswordHasher<IdentityUser>();
            adminUser.PasswordHash = hasher.HashPassword(adminUser, "123");

            builder.Entity<IdentityUser>().HasData(adminUser);
        }
    }
}
