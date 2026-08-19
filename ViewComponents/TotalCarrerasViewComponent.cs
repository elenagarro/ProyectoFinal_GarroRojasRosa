using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoFinal_GarroRojasRosa.Data;

namespace ProyectoFinal_GarroRojasRosa.ViewComponents
{
    public class TotalCarrerasViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public TotalCarrerasViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var total = await _context.Carreras.CountAsync();

            return View(total);
        }
    }
}