using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectoFinal_GarroRojasRosa.Data;
using ProyectoFinal_GarroRojasRosa.Models;

namespace ProyectoFinal_GarroRojasRosa.Controllers
{
    public class CursosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CursosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Cursos
        // Administrador y Estudiante pueden ver la lista
        [Authorize]
        public async Task<IActionResult> Index(
     string? buscar,
     int? idCarrera,
     int pagina = 1)
        {
            const int registrosPorPagina = 5;

            var consulta = _context.Cursos
                .Include(c => c.Carrera)
                .Include(c => c.Docente)
                .AsQueryable();

            // FILTRO POR NOMBRE O CÓDIGO
            if (!string.IsNullOrWhiteSpace(buscar))
            {
                consulta = consulta.Where(c =>
                    c.Nombre.Contains(buscar) ||
                    c.Codigo.Contains(buscar));
            }

            // FILTRO POR CARRERA
            if (idCarrera.HasValue && idCarrera.Value > 0)
            {
                consulta = consulta.Where(c =>
                    c.IdCarrera == idCarrera.Value);
            }

            var totalRegistros = await consulta.CountAsync();

            var totalPaginas = (int)Math.Ceiling(
                totalRegistros / (double)registrosPorPagina);

            if (pagina < 1)
            {
                pagina = 1;
            }

            if (totalPaginas > 0 && pagina > totalPaginas)
            {
                pagina = totalPaginas;
            }

            var cursos = await consulta
                .OrderBy(c => c.Nombre)
                .Skip((pagina - 1) * registrosPorPagina)
                .Take(registrosPorPagina)
                .ToListAsync();

            ViewBag.Buscar = buscar;

            ViewBag.IdCarreraFiltro = new SelectList(
                _context.Carreras
                    .Where(c => c.Estado)
                    .OrderBy(c => c.Nombre),
                "IdCarrera",
                "Nombre",
                idCarrera);

            ViewBag.PaginaActual = pagina;
            ViewBag.TotalPaginas = totalPaginas;

            return View(cursos);
        }

        // GET: Cursos/Details/5
        // Administrador y Estudiante pueden ver detalles
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var curso = await _context.Cursos
                .Include(c => c.Carrera)
                .Include(c => c.Docente)
                .FirstOrDefaultAsync(m => m.IdCurso == id);

            if (curso == null)
            {
                return NotFound();
            }

            return View(curso);
        }

        // GET: Cursos/Create
        // Solo Administrador
        [Authorize(Roles = "Administrador")]
        public IActionResult Create()
        {
            CargarCarreras();
            CargarDocentes();

            return View();
        }

        // POST: Cursos/Create
        // Solo Administrador
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Create(
            [Bind("IdCurso,Codigo,Nombre,Creditos,Estado,IdCarrera,IdDocente")] Curso curso)
        {
            if (ModelState.IsValid)
            {
                _context.Add(curso);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            CargarCarreras(curso.IdCarrera);
            CargarDocentes(curso.IdDocente);

            return View(curso);
        }

        // GET: Cursos/Edit/5
        // Solo Administrador
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var curso = await _context.Cursos.FindAsync(id);

            if (curso == null)
            {
                return NotFound();
            }

            CargarCarreras(curso.IdCarrera);
            CargarDocentes(curso.IdDocente);

            return View(curso);
        }

        // POST: Cursos/Edit/5
        // Solo Administrador
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("IdCurso,Codigo,Nombre,Creditos,Estado,IdCarrera,IdDocente")] Curso curso)
        {
            if (id != curso.IdCurso)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(curso);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CursoExists(curso.IdCurso))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            CargarCarreras(curso.IdCarrera);
            CargarDocentes(curso.IdDocente);

            return View(curso);
        }

        // GET: Cursos/Delete/5
        // Solo Administrador
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var curso = await _context.Cursos
                .Include(c => c.Carrera)
                .Include(c => c.Docente)
                .FirstOrDefaultAsync(m => m.IdCurso == id);

            if (curso == null)
            {
                return NotFound();
            }

            return View(curso);
        }

        // POST: Cursos/Delete/5
        // Solo Administrador
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var curso = await _context.Cursos.FindAsync(id);

            if (curso != null)
            {
                _context.Cursos.Remove(curso);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool CursoExists(int id)
        {
            return _context.Cursos.Any(e => e.IdCurso == id);
        }

        // ===============================
        // MÉTODOS AUXILIARES
        // ===============================

        private void CargarCarreras(int? carreraSeleccionada = null)
        {
            ViewBag.IdCarrera = new SelectList(
                _context.Carreras
                    .Where(c => c.Estado)
                    .OrderBy(c => c.Nombre),
                "IdCarrera",
                "Nombre",
                carreraSeleccionada);
        }

        private void CargarDocentes(int? docenteSeleccionado = null)
        {
            ViewBag.IdDocente = new SelectList(
                _context.Docentes
                    .Where(d => d.Estado)
                    .OrderBy(d => d.Nombre),
                "IdDocente",
                "Nombre",
                docenteSeleccionado);
        }
    }
}