using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace GRUPO_3_SC_701.Models
{
    public class Vehiculo
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Placa { get; set; }

        [Required]
        [MaxLength(50)]
        public string Modelo { get; set; }

        [Required]
        public int Capacidad { get; set; }

        [Required]
        public EstadoVehiculo Estado { get; set; } = EstadoVehiculo.Bueno;

        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        public string UsuarioRegistroId { get; set; }
        public IdentityUser UsuarioRegistro { get; set; }

        public ICollection<Horario> Horarios { get; set; }
    }
}
