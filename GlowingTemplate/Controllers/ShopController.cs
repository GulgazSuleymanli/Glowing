using GlowingTemplate.DAL;
using GlowingTemplate.Models;
using GlowingTemplate.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace GlowingTemplate.Controllers
{
    public class ShopController : Controller
    {
        private readonly AppDbContext _context;
        

        public ShopController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int page=1)
        {
            int take = 6;
            List<Product> products = _context.Products.Include(x=>x.ProductImages).ToList();
            ViewBag.TotalPage = (int)Math.Ceiling((double)products.Count / take);
            ViewBag.CurrentPage = page;
            return View(products);
        }
        public async Task<IActionResult> Detail(int id)
        {
            Product product=await _context.Products.Include(i => i.ProductImages)
                .Include(c => c.Category)
                .FirstOrDefaultAsync(p => p.Id == id);

            DetailVM detailVM = new DetailVM()
            {
                Product = product
            };

            return View(detailVM);
        }

        public IActionResult GetBasket()
        {
            var cookie = Request.Cookies["Basket"];
            return Content(cookie);
        }
    }
}
