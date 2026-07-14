using System.ComponentModel.DataAnnotations;

namespace ProyectoFinal_GarroRojasRosa.Models
{
    public class Docente
    {
        [Key]
        public int IdDocente { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;
    }
}