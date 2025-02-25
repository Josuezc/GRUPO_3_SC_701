using System.ComponentModel.DataAnnotations;

namespace GRUPO_3_SC_701.Models
{
    public class Parada
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int RutaId { get; set; }
        public Ruta Ruta { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; }

        [Required]
        public int Orden { get; set; }
    }
}
