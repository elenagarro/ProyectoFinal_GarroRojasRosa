using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoFinal_GarroRojasRosa.Models
{
    public class Matricula
    {
        [Key]
        public int IdMatricula { get; set; }

        [Required]
        [Display(Name = "Estudiante")]
        public int IdEstudiante { get; set; }

        [ForeignKey("IdEstudiante")]
        public Estudiante? Estudiante { get; set; }

        [Required]
        [Display(Name = "Curso")]
        public int IdCurso { get; set; }

        [ForeignKey("IdCurso")]
        public Curso? Curso { get; set; }

        [Display(Name = "Fecha de Matrícula")]
        public DateTime FechaMatricula { get; set; } = DateTime.Now;

        public bool Estado { get; set; } = true;
    }
}