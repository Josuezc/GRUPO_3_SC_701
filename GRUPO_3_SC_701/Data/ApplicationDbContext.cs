using GRUPO_3_SC_701.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

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
                .HasOne(rc => rc.Vehiculo)
                .WithMany() 
                .HasForeignKey(rc => rc.VehiculoId)
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


            var roles = new List<IdentityRole>
            {
                new IdentityRole { Id = "1", Name = "Administrador", NormalizedName = "ADMINISTRADOR" },
                new IdentityRole { Id = "2", Name = "Conductor", NormalizedName = "CONDUCTOR" },
                new IdentityRole { Id = "3", Name = "Usuario", NormalizedName = "USUARIO" }
            };
            builder.Entity<IdentityRole>().HasData(roles);

            var adminUser = new IdentityUser
            {
                Id = "1001",
                UserName = "admin@domain.com",
                Email = "admin@domain.com",
                NormalizedUserName = "ADMIN@DOMAIN.COM",
                NormalizedEmail = "ADMIN@DOMAIN.COM",
                EmailConfirmed = true
            };
            var userUser = new IdentityUser
            {
                Id = "1002",
                UserName = "user@domain.com",
                Email = "user@domain.com",
                NormalizedUserName = "USER@DOMAIN.COM",
                NormalizedEmail = "USER@DOMAIN.COM",
                EmailConfirmed = true
            };
            var clientUser = new IdentityUser
            {
                Id = "1003",
                UserName = "client@domain.com",
                Email = "client@domain.com",
                NormalizedUserName = "CLIENT@DOMAIN.COM",
                NormalizedEmail = "CLIENT@DOMAIN.COM",
                EmailConfirmed = true
            };

            var hasher = new PasswordHasher<IdentityUser>();
            adminUser.PasswordHash = hasher.HashPassword(adminUser, "Admin123!");
            userUser.PasswordHash = hasher.HashPassword(userUser, "User123!");
            clientUser.PasswordHash = hasher.HashPassword(clientUser, "Client123!");

            builder.Entity<IdentityUser>().HasData(adminUser);
            builder.Entity<IdentityUser>().HasData(userUser);
            builder.Entity<IdentityUser>().HasData(clientUser);



            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    UserId = "1001",RoleId = "1"

                },
                 new IdentityUserRole<string>
                 {
                     UserId = "1002",
                     RoleId = "2"

                 },
                  new IdentityUserRole<string>
                  {
                      UserId = "1003",
                      RoleId = "3"

                  }
            );

        }
    }
}
