using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoFinal_GarroRojasRosa.Data;

namespace ProyectoFinal_GarroRojasRosa.ViewComponents
{
    public class TotalCursosViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public TotalCursosViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var total = await _context.Cursos.CountAsync();

            return View(total);
        }
    }
}