using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoFinal_GarroRojasRosa.Models
{
    public class Estudiante
    {
        [Key]
        public int IdEstudiante { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "Ingrese un correo válido")]
        public string Correo { get; set; } = string.Empty;

        // RELACIÓN CON CARRERA
        [Required(ErrorMessage = "Debe seleccionar una carrera")]
        [Display(Name = "Carrera")]
        public int IdCarrera { get; set; }

        [ForeignKey("IdCarrera")]
        public Carrera? Carrera { get; set; }

        // RELACIÓN CON ASP.NET IDENTITY
        public string? ApplicationUserId { get; set; }

        [ForeignKey("ApplicationUserId")]
        public ApplicationUser? ApplicationUser { get; set; }
    }
}