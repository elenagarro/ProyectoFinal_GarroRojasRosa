using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProyectoFinal_GarroRojasRosa.Models;

namespace ProyectoFinal_GarroRojasRosa.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Carrera> Carreras { get; set; }
        public DbSet<Curso> Cursos { get; set; }
        public DbSet<Estudiante> Estudiantes { get; set; }
        public DbSet<Docente> Docentes { get; set; }
        public DbSet<Matricula> Matriculas { get; set; }
    }
}