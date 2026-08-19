using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoFinal_GarroRojasRosa.Data;

namespace ProyectoFinal_GarroRojasRosa.ViewComponents
{
    public class TotalEstudiantesViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public TotalEstudiantesViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var total = await _context.Estudiantes.CountAsync();

            return View(total);
        }
    }
}