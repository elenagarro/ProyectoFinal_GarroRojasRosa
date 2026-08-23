using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoFinal_GarroRojasRosa.Models
{
    public class Curso
    {
        [Key]
        public int IdCurso { get; set; }

        [Required(ErrorMessage = "El código es obligatorio")]
        [StringLength(20)]
        public string Codigo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "Los créditos son obligatorios")]
        [Range(1, 10, ErrorMessage = "Los créditos deben estar entre 1 y 10")]
        public int Creditos { get; set; }

        public bool Estado { get; set; } = true;

        // RELACIÓN CON CARRERA
        [Required(ErrorMessage = "Debe seleccionar una carrera")]
        [Display(Name = "Carrera")]
        public int IdCarrera { get; set; }

        [ForeignKey("IdCarrera")]
        public Carrera? Carrera { get; set; }

        // RELACIÓN CON DOCENTE
        [Display(Name = "Docente")]
        public int? IdDocente { get; set; }

        [ForeignKey("IdDocente")]
        public Docente? Docente { get; set; }
    }
}