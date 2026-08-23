using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoFinal_GarroRojasRosa.Data;
using ProyectoFinal_GarroRojasRosa.Models;

namespace ProyectoFinal_GarroRojasRosa.Controllers
{
    [Authorize]
    public class MatriculasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public MatriculasController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // ============================================
        // ADMINISTRADOR: VER TODAS LAS MATRÍCULAS
        // ============================================

        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Index()
        {
            var matriculas = await _context.Matriculas
                .Include(m => m.Estudiante)
                .Include(m => m.Curso)
                    .ThenInclude(c => c.Carrera)
                .OrderByDescending(m => m.FechaMatricula)
                .ToListAsync();

            return View(matriculas);
        }

        // ============================================
        // ESTUDIANTE: VER SUS MATRÍCULAS
        // ============================================

        [Authorize(Roles = "Estudiante")]
        public async Task<IActionResult> MisMatriculas()
        {
            var estudiante = await ObtenerEstudianteActual();

            if (estudiante == null)
            {
                return NotFound(
                    "No existe un registro de estudiante asociado a este usuario.");
            }

            var matriculas = await _context.Matriculas
                .Include(m => m.Curso)
                    .ThenInclude(c => c.Carrera)
                .Include(m => m.Curso)
                    .ThenInclude(c => c.Docente)
                .Where(m => m.IdEstudiante == estudiante.IdEstudiante)
                .OrderByDescending(m => m.FechaMatricula)
                .ToListAsync();

            return View(matriculas);
        }

        // ============================================
        // ESTUDIANTE: VER CURSOS DISPONIBLES
        // ============================================

        [Authorize(Roles = "Estudiante")]
        public async Task<IActionResult> CursosDisponibles()
        {
            var estudiante = await ObtenerEstudianteActual();

            if (estudiante == null)
            {
                return NotFound(
                    "No existe un registro de estudiante asociado a este usuario.");
            }

            // Cursos activos pertenecientes a la carrera del estudiante.
            var cursos = await _context.Cursos
                .Include(c => c.Carrera)
                .Include(c => c.Docente)
                .Where(c =>
                    c.Estado &&
                    c.IdCarrera == estudiante.IdCarrera)
                .OrderBy(c => c.Nombre)
                .ToListAsync();

            // Cursos en los que ya está matriculado.
            var cursosMatriculados = await _context.Matriculas
                .Where(m =>
                    m.IdEstudiante == estudiante.IdEstudiante &&
                    m.Estado)
                .Select(m => m.IdCurso)
                .ToListAsync();

            ViewBag.CursosMatriculados = cursosMatriculados;

            return View(cursos);
        }

        // ============================================
        // ESTUDIANTE: MATRICULAR UN CURSO
        // ============================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Estudiante")]
        public async Task<IActionResult> Matricular(int idCurso)
        {
            var estudiante = await ObtenerEstudianteActual();

            if (estudiante == null)
            {
                return NotFound();
            }

            var curso = await _context.Cursos
                .FirstOrDefaultAsync(c =>
                    c.IdCurso == idCurso &&
                    c.Estado);

            if (curso == null)
            {
                return NotFound();
            }

            // El estudiante solo puede matricular cursos de su carrera.
            if (curso.IdCarrera != estudiante.IdCarrera)
            {
                return Forbid();
            }

            // Evitar matrícula duplicada.
            var existeMatricula = await _context.Matriculas.AnyAsync(m =>
                m.IdEstudiante == estudiante.IdEstudiante &&
                m.IdCurso == idCurso &&
                m.Estado);

            if (existeMatricula)
            {
                TempData["Error"] =
                    "El estudiante ya se encuentra matriculado en este curso.";

                return RedirectToAction(nameof(CursosDisponibles));
            }

            var matricula = new Matricula
            {
                IdEstudiante = estudiante.IdEstudiante,
                IdCurso = idCurso,
                FechaMatricula = DateTime.Now,
                Estado = true
            };

            _context.Matriculas.Add(matricula);
            await _context.SaveChangesAsync();

            TempData["Mensaje"] =
                "La matrícula se realizó correctamente.";

            return RedirectToAction(nameof(MisMatriculas));
        }

        // ============================================
        // ESTUDIANTE: CANCELAR MATRÍCULA
        // ============================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Estudiante")]
        public async Task<IActionResult> Cancelar(int id)
        {
            var estudiante = await ObtenerEstudianteActual();

            if (estudiante == null)
            {
                return NotFound();
            }

            var matricula = await _context.Matriculas
                .FirstOrDefaultAsync(m =>
                    m.IdMatricula == id &&
                    m.IdEstudiante == estudiante.IdEstudiante);

            if (matricula == null)
            {
                return NotFound();
            }

            // No eliminamos el registro.
            // Lo dejamos inactivo para conservar el historial.
            matricula.Estado = false;

            await _context.SaveChangesAsync();

            TempData["Mensaje"] =
                "La matrícula fue cancelada correctamente.";

            return RedirectToAction(nameof(MisMatriculas));

        }

        // ============================================
        // ESTUDIANTE: MATRICULAR CURSO MEDIANTE AJAX
        // ============================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Estudiante")]
        public async Task<IActionResult> MatricularAjax(int idCurso)
        {
            var estudiante = await ObtenerEstudianteActual();

            if (estudiante == null)
            {
                return Json(new
                {
                    success = false,
                    mensaje = "No existe un estudiante asociado a este usuario."
                });
            }

            var curso = await _context.Cursos
                .FirstOrDefaultAsync(c =>
                    c.IdCurso == idCurso &&
                    c.Estado);

            if (curso == null)
            {
                return Json(new
                {
                    success = false,
                    mensaje = "El curso seleccionado no existe o está inactivo."
                });
            }

            // Solo puede matricular cursos de su propia carrera
            if (curso.IdCarrera != estudiante.IdCarrera)
            {
                return Json(new
                {
                    success = false,
                    mensaje = "Este curso no pertenece a su carrera."
                });
            }

            // Evitar matrícula duplicada
            var existeMatricula = await _context.Matriculas
                .AnyAsync(m =>
                    m.IdEstudiante == estudiante.IdEstudiante &&
                    m.IdCurso == idCurso &&
                    m.Estado);

            if (existeMatricula)
            {
                return Json(new
                {
                    success = false,
                    mensaje = "Ya se encuentra matriculado en este curso."
                });
            }

            var matricula = new Matricula
            {
                IdEstudiante = estudiante.IdEstudiante,
                IdCurso = idCurso,
                FechaMatricula = DateTime.Now,
                Estado = true
            };

            _context.Matriculas.Add(matricula);

            await _context.SaveChangesAsync();

            return Json(new
            {
                success = true,
                mensaje = "La matrícula se realizó correctamente."
            });
        }

        // ============================================
        // MÉTODO AUXILIAR
        // ============================================

        private async Task<Estudiante?> ObtenerEstudianteActual()
        {
            var usuario = await _userManager.GetUserAsync(User);

            if (usuario == null)
            {
                return null;
            }

            // Primero buscar por la relación directa con Identity
            var estudiante = await _context.Estudiantes
                .Include(e => e.Carrera)
                .FirstOrDefaultAsync(
                    e => e.ApplicationUserId == usuario.Id);

            if (estudiante != null)
            {
                return estudiante;
            }

            // Compatibilidad con estudiantes creados antes
            // de implementar ApplicationUserId
            estudiante = await _context.Estudiantes
                .Include(e => e.Carrera)
                .FirstOrDefaultAsync(
                    e => e.Correo == usuario.Email);

            if (estudiante != null)
            {
                // Vincular automáticamente el registro antiguo
                // con la cuenta de ASP.NET Identity
                estudiante.ApplicationUserId = usuario.Id;

                await _context.SaveChangesAsync();

                return estudiante;
            }

            return null;
        }
    }
}