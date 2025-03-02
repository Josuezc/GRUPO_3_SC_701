using GRUPO_3_SC_701.Data;
using GRUPO_3_SC_701.Models;
using System.ComponentModel.DataAnnotations;

namespace GRUPO_3_SC_701.Validation
{
    public class UniqueOrden : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var model = (Parada)validationContext.ObjectInstance;
            var _context = (ApplicationDbContext)validationContext.GetService(typeof(ApplicationDbContext));

            bool existeParada = _context.Paradas.Any(p => p.RutaId == model.RutaId && p.Orden == (int)value);

            if (existeParada)
            {
                return new ValidationResult("Esta ruta ya tiene una parada con el mismo orden. Seleccione otro orden.");
            }

            return ValidationResult.Success;
        }
    }
}
