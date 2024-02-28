using GlowingTemplate.DAL;
using GlowingTemplate.Helpers;
using GlowingTemplate.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GlowingTemplate.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class SectionController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SectionController(AppDbContext context, IWebHostEnvironment env)
        {
            this._context = context;
            this._env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<Section> sections = await _context.Sections.ToListAsync();
            return View(sections);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]

        public async Task<IActionResult> Create(Section section)
        {
            if (section == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (section.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "Add a Picture");
                return View();
            }
            if (!section.ImageFile.CheckType("image/"))
            {
                ModelState.AddModelError("ImageFile", "Add a Picture");
                return View();
            }
            if (section.ImageFile.CheckLength(200))
            {
                ModelState.AddModelError("ImageFile", "The size of the image cannot exceed 200 MB");
            }

            section.ImageUrl = section.ImageFile.CreateFile(_env.WebRootPath, @"upload/section");

            await _context.Sections.AddAsync(section);

            await _context.SaveChangesAsync();


            return RedirectToAction("Index");
        }


        public IActionResult Update(int id)
        {
            if (id == null) return BadRequest();
            Section newsection = _context.Sections.FirstOrDefault(s => s.Id == id);
            if (newsection == null) return NotFound();
            return View(newsection);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, Section section)
        {
            if (id == null) return BadRequest();
            Section newsection = _context.Sections.FirstOrDefault(s => s.Id == id);
            if (newsection == null) return NotFound();
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (section.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "Add a Picture");
                return View();
            }
            if (!section.ImageFile.CheckType("image/"))
            {
                ModelState.AddModelError("ImageFile", "Add a Picture");
                return View();
            }
            if (section.ImageFile.CheckLength(200))
            {
                ModelState.AddModelError("ImageFile", "The size of the image cannot exceed 200 MB");
                return View();
            }

            newsection.ImageUrl.DeleteFile(_env.WebRootPath, "upload/section");
            newsection.ImageUrl = section.ImageFile.CreateFile(_env.WebRootPath, "upload/section");

            newsection.Title = section.Title;
            newsection.Description = section.Description;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");

        }

        public async Task<IActionResult> Delete(int id)
        {
            if (id == null) return BadRequest();
            Section section = _context.Sections.FirstOrDefault(s => s.Id == id);
            if (section == null) return NotFound();
            section.ImageUrl.DeleteFile(_env.WebRootPath, "upload/section");
            _context.Sections.Remove(section);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");

        }

    }
}
