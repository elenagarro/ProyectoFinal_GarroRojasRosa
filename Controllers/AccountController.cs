using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoFinal_GarroRojasRosa.Data;
using ProyectoFinal_GarroRojasRosa.Models;

namespace ProyectoFinal_GarroRojasRosa.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        // ================= LOGIN =================

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var resultado = await _signInManager.PasswordSignInAsync(
                model.Email,
                model.Password,
                model.RememberMe,
                lockoutOnFailure: false);

            if (resultado.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(
                "",
                "Correo o contraseña incorrectos.");

            return View(model);
        }

        // ================= REGISTER =================

        [HttpGet]
        public IActionResult Register()
        {
            ViewBag.IdCarrera = new SelectList(
                _context.Carreras
                    .Where(c => c.Estado),
                "IdCarrera",
                "Nombre");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                CargarCarreras(model.IdCarrera);
                return View(model);
            }

            // Crear usuario de Identity
            var usuario = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                Nombre = model.Nombre,
                Apellido = model.Apellido
            };

            var resultadoUsuario = await _userManager.CreateAsync(
                usuario,
                model.Password);

            if (!resultadoUsuario.Succeeded)
            {
                foreach (var error in resultadoUsuario.Errors)
                {
                    ModelState.AddModelError(
                        "",
                        error.Description);
                }

                CargarCarreras(model.IdCarrera);

                return View(model);
            }

            // Asignar automáticamente el rol Estudiante
            var resultadoRol = await _userManager.AddToRoleAsync(
                usuario,
                "Estudiante");

            if (!resultadoRol.Succeeded)
            {
                foreach (var error in resultadoRol.Errors)
                {
                    ModelState.AddModelError(
                        "",
                        error.Description);
                }

                // Si falló la asignación del rol,
                // eliminamos el usuario que acabamos de crear
                await _userManager.DeleteAsync(usuario);

                CargarCarreras(model.IdCarrera);

                return View(model);
            }

            // Crear registro académico del estudiante
            var estudiante = new Estudiante
            {
                Nombre = $"{model.Nombre} {model.Apellido}",
                Correo = model.Email,
                IdCarrera = model.IdCarrera,
                ApplicationUserId = usuario.Id
            };

            try
            {
                _context.Estudiantes.Add(estudiante);
                await _context.SaveChangesAsync();
            }
            catch
            {
                // Evita dejar un usuario de Identity
                // sin registro de Estudiante si falla la BD
                await _userManager.RemoveFromRoleAsync(
                    usuario,
                    "Estudiante");

                await _userManager.DeleteAsync(usuario);

                ModelState.AddModelError(
                    "",
                    "No fue posible completar el registro del estudiante.");

                CargarCarreras(model.IdCarrera);

                return View(model);
            }

            // Iniciar sesión automáticamente
            await _signInManager.SignInAsync(
                usuario,
                isPersistent: false);

            return RedirectToAction(
                "Index",
                "Home");
        }

        // ================= LOGOUT =================

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction(
                "Index",
                "Home");
        }

        // ================= MÉTODO AUXILIAR =================

        private void CargarCarreras(int? carreraSeleccionada = null)
        {
            ViewBag.IdCarrera = new SelectList(
                _context.Carreras
                    .Where(c => c.Estado),
                "IdCarrera",
                "Nombre",
                carreraSeleccionada);
        }
    }
}