using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoFinal_GarroRojasRosa.Data;
using ProyectoFinal_GarroRojasRosa.Models;
using ProyectoFinal_GarroRojasRosa.Services;

namespace ProyectoFinal_GarroRojasRosa.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class FirestoreController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly FirestoreService _firestoreService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public FirestoreController(
            ApplicationDbContext context,
            FirestoreService firestoreService,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _firestoreService = firestoreService;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Sincronizar()
        {
            // 1. CARRERAS
            var carreras = await _context.Carreras.ToListAsync();

            foreach (var carrera in carreras)
            {
                var datos = new Dictionary<string, object>
                {
                    { "nombre", carrera.Nombre },
                    { "descripcion", carrera.Descripcion ?? "" },
                    { "estado", carrera.Estado }
                };

                await _firestoreService.GuardarDocumentoAsync(
                    "carreras",
                    carrera.IdCarrera.ToString(),
                    datos);
            }


            // 2. CURSOS
            var cursos = await _context.Cursos
                .Include(c => c.Carrera)
                .Include(c => c.Docente)
                .ToListAsync();

            foreach (var curso in cursos)
            {
                var datos = new Dictionary<string, object>
                {
                    { "codigo", curso.Codigo },
                    { "nombre", curso.Nombre },
                    { "creditos", curso.Creditos },
                    { "estado", curso.Estado },
                    { "carrera", curso.Carrera?.Nombre ?? "" },
                    { "docente", curso.Docente?.Nombre ?? "Sin docente asignado" }
                };

                await _firestoreService.GuardarDocumentoAsync(
                    "cursos",
                    curso.IdCurso.ToString(),
                    datos);
            }


            // 3. DOCENTES
            var docentes = await _context.Docentes.ToListAsync();

            foreach (var docente in docentes)
            {
                var datos = new Dictionary<string, object>
                {
                    { "nombre", docente.Nombre },
                    { "correo", docente.Correo },
                    { "especialidad", docente.Especialidad },
                    { "estado", docente.Estado }
                };

                await _firestoreService.GuardarDocumentoAsync(
                    "docentes",
                    docente.IdDocente.ToString(),
                    datos);
            }


            // 4. ESTUDIANTES
            var estudiantes = await _context.Estudiantes
                .Include(e => e.Carrera)
                .ToListAsync();

            foreach (var estudiante in estudiantes)
            {
                var datos = new Dictionary<string, object>
                {
                    { "nombre", estudiante.Nombre },
                    { "correo", estudiante.Correo },
                    { "carrera", estudiante.Carrera?.Nombre ?? "" },
                    { "applicationUserId", estudiante.ApplicationUserId ?? "" }
                };

                await _firestoreService.GuardarDocumentoAsync(
                    "estudiantes",
                    estudiante.IdEstudiante.ToString(),
                    datos);
            }


            // 5. MATRÍCULAS
            var matriculas = await _context.Matriculas
                .Include(m => m.Estudiante)
                .Include(m => m.Curso)
                .ToListAsync();

            foreach (var matricula in matriculas)
            {
                var datos = new Dictionary<string, object>
    {
        { "estudiante", matricula.Estudiante?.Nombre ?? "" },
        { "curso", matricula.Curso?.Nombre ?? "" },
        {
            "fechaMatricula",
            DateTime.SpecifyKind(
                matricula.FechaMatricula,
                DateTimeKind.Local)
                .ToUniversalTime()
        },
        { "estado", matricula.Estado }
    };

                await _firestoreService.GuardarDocumentoAsync(
                    "matriculas",
                    matricula.IdMatricula.ToString(),
                    datos);
            }

            // 6. USUARIOS
            var usuarios = await _userManager.Users.ToListAsync();

            foreach (var usuario in usuarios)
            {
                var rolesUsuario =
                    await _userManager.GetRolesAsync(usuario);

                var datos = new Dictionary<string, object>
                {
                    { "nombre", usuario.Nombre ?? "" },
                    { "apellido", usuario.Apellido ?? "" },
                    { "correo", usuario.Email ?? "" },
                    { "roles", rolesUsuario.ToList() }
                };

                await _firestoreService.GuardarDocumentoAsync(
                    "usuarios",
                    usuario.Id,
                    datos);
            }


            // 7. ROLES
            var roles = await _roleManager.Roles.ToListAsync();

            foreach (var rol in roles)
            {
                var datos = new Dictionary<string, object>
                {
                    { "nombre", rol.Name ?? "" },
                    { "nombreNormalizado", rol.NormalizedName ?? "" }
                };

                await _firestoreService.GuardarDocumentoAsync(
                    "roles",
                    rol.Id,
                    datos);
            }


            // 8. AUDITORÍA
            var usuarioActual =
                await _userManager.GetUserAsync(User);

            var auditoria = new Dictionary<string, object>
            {
                { "accion", "Sincronización general con Firestore" },
                { "usuario", usuarioActual?.Email ?? "Administrador" },
                { "fecha", DateTime.UtcNow },
                { "colecciones", 8 }
            };

            await _firestoreService.GuardarDocumentoAsync(
                "auditoria",
                Guid.NewGuid().ToString(),
                auditoria);


            TempData["Mensaje"] =
                "La información se sincronizó correctamente con Firestore.";

            return RedirectToAction(nameof(Index));
        }
    }
}