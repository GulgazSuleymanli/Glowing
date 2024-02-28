using GlowingTemplate.DAL;
using GlowingTemplate.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GlowingTemplate.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class SettingController : Controller
    {
        private readonly AppDbContext _context;

        public SettingController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            List<Setting> settings = await _context.Settings.ToListAsync();

            if (settings != null)
            {
                return View(settings);
            }
            else
            {
                return NotFound();
            }
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Setting setting)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (await _context.Settings.FirstOrDefaultAsync(x => x.Key == setting.Key) != null)
            {
                ModelState.AddModelError("Key", "Key can not be same");
                return View(setting);
            }

            _context.Settings.Add(setting);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }



        public async Task<IActionResult> Update(int id)
        {
            Setting setting = await _context.Settings.FirstOrDefaultAsync(x => x.Id == id);
            return View(setting);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, Setting setting)
        {
            Setting existSetting = await _context.Settings.FirstOrDefaultAsync(c => c.Id == setting.Id);
            if (!ModelState.IsValid)
            {
                return View(existSetting);
            }

            if (await _context.Settings.FirstOrDefaultAsync(x => x.Key == setting.Key && x.Id != setting.Id) != null)
            {
                ModelState.AddModelError("Key", "Key can not be same");
                return View(setting);
            }

            if (setting.Value != null)
            {
                existSetting.Value = setting.Value;
            }

            existSetting.Key = setting.Key;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


  
        public async Task<IActionResult> Delete(int id)
        {
            Setting setting = await _context.Settings.FirstOrDefaultAsync(c => c.Id == id);
            if (setting != null)
            {
                _context.Settings.Remove(setting);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            else
            {
                return NotFound();
            }
        }

    }
}

