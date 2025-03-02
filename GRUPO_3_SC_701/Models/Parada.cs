using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GRUPO_3_SC_701.Models
{
    public class Parada
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "El valor no puede ser negativo.")]
        public int RutaId { get; set; }

        [ForeignKey("RutaId")]
        public Ruta? Ruta { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "El valor no puede ser negativo.")]
        [Validation.UniqueOrden]
        public int Orden { get; set; }
    }
}
