using GlowingTemplate.DAL;
using GlowingTemplate.Models;
using Microsoft.AspNetCore.Mvc;

namespace GlowingTemplate.Controllers
{
    public class FAQController : Controller
    {
        AppDbContext _context;
        public FAQController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            List<FAQ> faq=_context.FAQs.ToList();
            return View(faq);
        }
    }
}
