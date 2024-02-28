using GlowingTemplate.DAL;
using GlowingTemplate.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GlowingTemplate.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            this._context = context;
        }
        public async Task<IActionResult> Index()
        {

            HomeVM homeVm = new HomeVM()
            {
                Sliders = await _context.Sliders.OrderByDescending(s => s.Id).Take(3).ToListAsync(),
                Sections = await _context.Sections.OrderByDescending(s => s.Id).Take(4).ToListAsync(),
                Products = await _context.Products.Include(p => p.ProductImages).ToListAsync(),
                Categories = await _context.Categories.Take(4).ToListAsync()
            };
            return View(homeVm);
        }
    }
}
