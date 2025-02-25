using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace GRUPO_3_SC_701.Models
{
    public class Boleto
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UsuarioId { get; set; }
        public IdentityUser Usuario { get; set; }

        [Required]
        public int HorarioId { get; set; }
        public Horario Horario { get; set; }

        public DateTime FechaCompra { get; set; } = DateTime.Now;
    }
}
