using System.ComponentModel.DataAnnotations;

namespace ProyectoFinal_GarroRojasRosa.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Ingrese el correo.")]
        [EmailAddress]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "Ingrese la contraseña.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = "";

        public bool RememberMe { get; set; }
    }
}