using GRUPO_3_SC_701.Data;
using GRUPO_3_SC_701.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace GRUPO_3_SC_701.Validation
{
    public class UniqueOrden : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var model = (Parada)validationContext.ObjectInstance;
            var _context = (ApplicationDbContext)validationContext.GetService(typeof(ApplicationDbContext));

            var paradaExistente = _context.Paradas
                .AsNoTracking()
                .FirstOrDefault(p => p.Id == model.Id);

            if (paradaExistente != null && paradaExistente.Orden == (int)value)
            {
                return ValidationResult.Success;
            }

            bool existeParada = _context.Paradas.Any(p => p.RutaId == model.RutaId && p.Orden == (int)value && p.Id != model.Id);

            if (existeParada)
            {
                return new ValidationResult("Esta ruta ya tiene una parada con el mismo orden. Seleccione otro orden.");
            }

            return ValidationResult.Success;
        }
    }
}
