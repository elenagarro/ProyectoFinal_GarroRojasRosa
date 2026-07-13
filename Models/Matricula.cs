using System.ComponentModel.DataAnnotations;

namespace ProyectoFinal_GarroRojasRosa.Models
{
    public class Matricula
    {
        [Key]
        public int IdMatricula { get; set; }

        public int IdEstudiante { get; set; }

        public int IdCarrera { get; set; }
    }
}