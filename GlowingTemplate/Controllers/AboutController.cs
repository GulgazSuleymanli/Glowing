using Microsoft.AspNetCore.Mvc;

namespace GlowingTemplate.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
