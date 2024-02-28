using GlowingTemplate.DAL;
using GlowingTemplate.Helpers;
using GlowingTemplate.Models;
using Microsoft.AspNetCore.Mvc;

namespace GlowingTemplate.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public CategoryController(AppDbContext context,IWebHostEnvironment env)
        {
            _context = context;
            this._env = env;
        }
        public IActionResult Index()
        {
            List<Category> categories= _context.Categories.ToList();
            return View(categories);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            if (category == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (category.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "Add a Picture");
                return View();
            }
            if (!category.ImageFile.CheckType("image/"))
            {
                ModelState.AddModelError("ImageFile", "Add a Picture");
                return View();
            }
            if (category.ImageFile.CheckLength(200))
            {
                ModelState.AddModelError("ImageFile", "The size of the image cannot exceed 200 MB");
                return View();
            }
            category.ImageUrl = category.ImageFile.CreateFile(_env.WebRootPath, @"upload/category");

            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();


            return RedirectToAction("Index");
        }
        public IActionResult Update(int id)
        {
            if (id == null) return BadRequest();
            Category newcategory = _context.Categories.FirstOrDefault(s => s.Id == id);
            if (newcategory == null) return NotFound();
            return View(newcategory);
        }
        [HttpPost]
        public async Task<IActionResult> Update(Category category,int id)
        {
            if (id == null) return BadRequest();
            Category newcategory = _context.Categories.FirstOrDefault(s => s.Id == id);
            if (newcategory == null) return NotFound();
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (category.ImageFile != null)
            {
                if (!category.ImageFile.CheckType("image/"))
                {
                    ModelState.AddModelError("ImageFile", "Add a Picture");
                    return View();
                }
                if (!category.ImageFile.CheckLength(200))
                {
                    ModelState.AddModelError("ImageFile", "The size of the image cannot exceed 200 MB");
                    return View();
                }

                newcategory.ImageUrl.DeleteFile(_env.WebRootPath, "upload/category");
                newcategory.ImageUrl = category.ImageFile.CreateFile(_env.WebRootPath, "upload/category");
            }
            

            newcategory.Name = category.Name;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
          
        }
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null) return BadRequest();
            Category category = _context.Categories.FirstOrDefault(s => s.Id == id);
            if (category == null) return NotFound();
            category.ImageUrl.DeleteFile(_env.WebRootPath, "upload/category");
            _context.Categories.Remove(category);
          
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");

        }
    }
}
