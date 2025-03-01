using System.ComponentModel.DataAnnotations;

namespace GRUPO_3_SC_701.ViewModels
{
    public class UserViewModel
    {
        [Required]
        [Display(Name = "Nombre de Usuario")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Correo")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Teléfono")]
        public string PhoneNumber { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Rol")]
        public string Role { get; set; }

        [Display(Name = "Usuario Activo (Correo Confirmado)")]
        public bool EmailConfirmed { get; set; }
    }
}
