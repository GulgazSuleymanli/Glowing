using GlowingTemplate.DAL;
using GlowingTemplate.Helpers;
using GlowingTemplate.Models;
using GlowingTemplate.ViewModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GlowingTemplate.Controllers
{
    [AutoValidateAntiforgeryToken]
    [Area("Manage")]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._roleManager = roleManager;
        }
        public IActionResult Register()
        {


            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            AppUser user = new AppUser()
            {
                Email = registerVM.Email,
                Name = registerVM.Name,
                Surname = registerVM.Surname,
                UserName = registerVM.Username

            };

            IdentityResult result = await _userManager.CreateAsync(user, registerVM.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                    return View();
                }
            }

            //await _roleManager.AddToRoleAsync("Admin");
            return RedirectToAction("Index", "Home", new { area = "" });

        }
        public IActionResult Login()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var exist = await _userManager.FindByEmailAsync(loginVM.UserName);
            if (exist == null)
            {
                exist = await _userManager.FindByNameAsync(loginVM.UserName);

                if (exist == null)
                {
                    ModelState.AddModelError("", "UserName ve ya Password yalnisdir");
                    return View();
                }
            }

            var SignInCheck = await _signInManager.CheckPasswordSignInAsync(exist, loginVM.Password, true);
            if (!SignInCheck.Succeeded)
            {
                ModelState.AddModelError("", "UserName ve ya Password yalnisdir");
                return View();
            }

            if (SignInCheck.IsLockedOut)
            {
                ModelState.AddModelError("", "Birazdan yeniden cehd dersiz");
                return View();
            }

            await _signInManager.SignInAsync(exist, loginVM.RememberMe);

            return RedirectToAction("Index", "Home", new { area = "" });

        }

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> CreateRole()
        {
            var role = new IdentityRole("Admin");
            await _roleManager.CreateAsync(role);
            role = new IdentityRole("Member");
            await _roleManager.CreateAsync(role);
            return RedirectToAction("Index", "Home");

        }

    }
}



