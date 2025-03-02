using GRUPO_3_SC_701.Data;
using GRUPO_3_SC_701.Models;
using System.ComponentModel.DataAnnotations;


namespace GRUPO_3_SC_701.Validation
{
    public class UniqueCodRuta : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var model = (Ruta)validationContext.ObjectInstance;
            var _context = (ApplicationDbContext)validationContext.GetService(typeof(ApplicationDbContext));

            bool existeRuta = _context.Rutas.Any(r => r.Codigo == (string)value);

            if (existeRuta)
            {
                return new ValidationResult("Ya existe una ruta con este código. Elija otro.");
            }

            return ValidationResult.Success;
        }
    }
}
