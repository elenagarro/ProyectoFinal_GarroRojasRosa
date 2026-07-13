using System.ComponentModel.DataAnnotations;

namespace ProyectoFinal_GarroRojasRosa.Models
{
    public class Curso
    {
        [Key]
        public int IdCurso { get; set; }

        [Required(ErrorMessage = "El código es obligatorio.")]
        [StringLength(20)]
        public string Codigo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre del curso es obligatorio.")]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        public int Creditos { get; set; }

        public bool Estado { get; set; } = true;
    }
}