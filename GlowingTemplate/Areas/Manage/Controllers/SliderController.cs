using GlowingTemplate.DAL;
using GlowingTemplate.Helpers;
using GlowingTemplate.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GlowingTemplate.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class SliderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SliderController(AppDbContext context, IWebHostEnvironment env)
        {
            this._context = context;
            this._env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<Slider> slider = await _context.Sliders.ToListAsync();
            return View(slider);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]

        public async Task<IActionResult> Create(Slider slider)
        {
            if (slider == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (slider.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "Add a Picture");
                return View();
            }
            if (!slider.ImageFile.CheckType("image/"))
            {
                ModelState.AddModelError("ImageFile", "Add a Picture");
                return View();
            }
            if (slider.ImageFile.CheckLength(200))
            {
                ModelState.AddModelError("ImageFile", "The size of the image cannot exceed 200 MB");
            }

            slider.ImageUrl = slider.ImageFile.CreateFile(_env.WebRootPath, @"upload/slider");

            await _context.Sliders.AddAsync(slider);

            await _context.SaveChangesAsync();


            return RedirectToAction("Index");
        }


        public IActionResult Update(int id)
        {
            if (id == null) return BadRequest();
            Slider Slider = _context.Sliders.FirstOrDefault(s => s.Id == id);
            if (Slider == null) return NotFound();
            return View(Slider);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, Slider slider)
        {
            if (id == null) return BadRequest();
            Slider Slider = _context.Sliders.FirstOrDefault(s => s.Id == id);
            if (Slider == null) return NotFound();
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (slider.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "Add a Picture");
                return View();

            }

            if (!slider.ImageFile.CheckType("image/"))
            {
                ModelState.AddModelError("ImageFile", "Add a Picture");
                return View();
            }
            if (slider.ImageFile.CheckLength(200))
            {
                ModelState.AddModelError("ImageFile", "The size of the image cannot exceed 200 MB");
            }

            Slider.ImageUrl.DeleteFile(_env.WebRootPath, "upload/slider");
            Slider.ImageUrl = slider.ImageFile.CreateFile(_env.WebRootPath,"upload/slider");

            Slider.Title = slider.Title;
            Slider.Description = slider.Description;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");

        }

        public async Task<IActionResult> Delete(int id)
        {
            if (id == null) return BadRequest();
            Slider slider = _context.Sliders.FirstOrDefault(s => s.Id == id);
            if (slider == null) return NotFound();
            slider.ImageUrl.DeleteFile(_env.WebRootPath, "upload/slider");
            _context.Sliders.Remove(slider);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");

        }
    }
}
