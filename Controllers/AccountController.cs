using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProyectoFinal_GarroRojasRosa.Models;

namespace ProyectoFinal_GarroRojasRosa.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // ================= LOGIN =================

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var resultado = await _signInManager.PasswordSignInAsync(
                model.Email,
                model.Password,
                model.RememberMe,
                false);

            if (resultado.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Correo o contraseña incorrectos.");
            return View(model);
        }

        // ================= REGISTER =================

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            ApplicationUser usuario = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                Nombre = model.Nombre,
                Apellido = model.Apellido
            };

            var resultado = await _userManager.CreateAsync(usuario, model.Password);

            if (resultado.Succeeded)
            {
                await _signInManager.SignInAsync(usuario, false);

                return RedirectToAction("Index", "Home");
            }

            foreach (var error in resultado.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }

        // ================= LOGOUT =================

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}