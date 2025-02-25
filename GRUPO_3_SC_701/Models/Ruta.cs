using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace GRUPO_3_SC_701.Models
{

    public enum EstadoVehiculo
    {
        Bueno,
        Regular,
        NecesitaMantenimiento
    }

    public class Ruta
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Codigo { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        [Required]
        public bool Estado { get; set; }

        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        public string UsuarioRegistroId { get; set; }
        public IdentityUser UsuarioRegistro { get; set; }

        public ICollection<Parada> Paradas { get; set; }
        public ICollection<Horario> Horarios { get; set; }
        public ICollection<RutaConductor> RutaConductores { get; set; }
    }
}
