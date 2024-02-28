using GlowingTemplate.DAL;
using GlowingTemplate.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GlowingTemplate.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class FAQController : Controller
    {
        
        AppDbContext _context;

        public FAQController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            List<FAQ> faq=await _context.FAQs.ToListAsync();
            return View(faq);
        }


        public IActionResult Create()
        {
            return View();

        }
        [HttpPost]
        public IActionResult Create(FAQ faq)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            _context.FAQs.Add(faq);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Update(int id)
        {
            FAQ faq = _context.FAQs.Find(id);
            return View(faq);

        }
        [HttpPost]
        public IActionResult Update(FAQ faq)
        {
            FAQ exist = _context.FAQs.Find(faq.Id);
            exist.Question = faq.Question;
            exist.Answer = faq.Answer;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            FAQ faq = _context.FAQs.Find(id);
            _context.FAQs.Remove(faq);
            _context.SaveChanges();
            return RedirectToAction("Index");

        }
    }
}

