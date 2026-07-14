using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectoFinal_GarroRojasRosa.Data;
using ProyectoFinal_GarroRojasRosa.Models;

namespace ProyectoFinal_GarroRojasRosa.Controllers
{
    public class EstudiantesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EstudiantesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Estudiantes
        public async Task<IActionResult> Index()
        {
            var estudiantes = _context.Estudiantes
                .Include(e => e.Carrera);

            return View(await estudiantes.ToListAsync());
        }

        // GET: Estudiantes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estudiante = await _context.Estudiantes
                .Include(e => e.Carrera)
                .FirstOrDefaultAsync(e => e.IdEstudiante == id);

            if (estudiante == null)
            {
                return NotFound();
            }

            return View(estudiante);
        }

        // GET: Estudiantes/Create
        public IActionResult Create()
        {
            ViewBag.IdCarrera = new SelectList(
                _context.Carreras,
                "IdCarrera",
                "Nombre");

            return View();
        }

        // POST: Estudiantes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdEstudiante,Nombre,Correo,IdCarrera")] Estudiante estudiante)
        {
            if (ModelState.IsValid)
            {
                _context.Add(estudiante);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.IdCarrera = new SelectList(
                _context.Carreras,
                "IdCarrera",
                "Nombre",
                estudiante.IdCarrera);

            return View(estudiante);
        }

        // GET: Estudiantes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estudiante = await _context.Estudiantes.FindAsync(id);

            if (estudiante == null)
            {
                return NotFound();
            }

            ViewBag.IdCarrera = new SelectList(
                _context.Carreras,
                "IdCarrera",
                "Nombre",
                estudiante.IdCarrera);

            return View(estudiante);
        }

        // POST: Estudiantes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdEstudiante,Nombre,Correo,IdCarrera")] Estudiante estudiante)
        {
            if (id != estudiante.IdEstudiante)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(estudiante);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EstudianteExists(estudiante.IdEstudiante))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewBag.IdCarrera = new SelectList(
                _context.Carreras,
                "IdCarrera",
                "Nombre",
                estudiante.IdCarrera);

            return View(estudiante);
        }

        // GET: Estudiantes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estudiante = await _context.Estudiantes
                .Include(e => e.Carrera)
                .FirstOrDefaultAsync(e => e.IdEstudiante == id);

            if (estudiante == null)
            {
                return NotFound();
            }

            return View(estudiante);
        }

        // POST: Estudiantes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var estudiante = await _context.Estudiantes.FindAsync(id);

            if (estudiante != null)
            {
                _context.Estudiantes.Remove(estudiante);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool EstudianteExists(int id)
        {
            return _context.Estudiantes.Any(e => e.IdEstudiante == id);
        }
    }
}