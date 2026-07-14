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
        [EmailAddress]
        public string Correo { get; set; } = string.Empty;

        [Display(Name = "Carrera")]
        public int IdCarrera { get; set; }

        [ForeignKey("IdCarrera")]
        public Carrera? Carrera { get; set; }
    }
}
