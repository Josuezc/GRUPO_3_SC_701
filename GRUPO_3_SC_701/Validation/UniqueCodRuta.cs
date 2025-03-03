using GRUPO_3_SC_701.Data;
using GRUPO_3_SC_701.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;


namespace GRUPO_3_SC_701.Validation
{
    public class UniqueCodRuta : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var model = (Ruta)validationContext.ObjectInstance;
            var _context = (ApplicationDbContext)validationContext.GetService(typeof(ApplicationDbContext));

            var rutaExistente = _context.Rutas
                .AsNoTracking()
                .FirstOrDefault(r => r.Id == model.Id);

            if (rutaExistente != null && rutaExistente.Codigo == (string)value)
            {
                return ValidationResult.Success;
            }

            bool existeRuta = _context.Rutas.Any(r => r.Codigo == (string)value && r.Id != model.Id);

            if (existeRuta)
            {
                return new ValidationResult("Ya existe una ruta con este código. Elija otro.");
            }

            return ValidationResult.Success;
        }
    }
}
