using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace GRUPO_3_SC_701.Models
{
    public class RutaConductor
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int RutaId { get; set; }
        public Ruta Ruta { get; set; }

        [Required]
        public int VehiculoId { get; set; }
        public Vehiculo? Vehiculo { get; set; }
    }
}
