using GlowingTemplate.DAL;
using GlowingTemplate.Models;
using GlowingTemplate.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;

namespace GlowingTemplate.Controllers
{
    public class CartController : Controller
    {
        AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public CartController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {


            List<BasketCookieVM> basketItems = new List<BasketCookieVM>();

            if (User.Identity.IsAuthenticated)
            {
                AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
                List<BasketItem> userBasket = await _context.BasketItems
                    .Where(b => b.AppUserId == user.Id && b.OrderId == null)
                    .Include(c=>c.Product)
                    .ThenInclude(p => p.ProductImages
                    .Where(pi => pi.IsPrime == true)).ToListAsync();
                foreach (var item in userBasket)
                {
                    basketItems.Add(new BasketCookieVM()
                    {
                       Count= item.Count,
                       Id= item.Id,
                    });
                }
            }
            else
            {


                List<BasketCookieVM> basketCookies = new List<BasketCookieVM>();
                if (Request.Cookies["Basket"] != null)
                {
                    basketCookies = JsonConvert.DeserializeObject<List<BasketCookieVM>>(Request.Cookies["Basket"]);
                    foreach (var item in basketCookies)
                    {
                        Product product = _context.Products.Where(p => p.IsDeleted == false).Include(p => p.ProductImages.Where(pi => pi.IsPrime == true)).FirstOrDefault(p => p.Id == item.Id);

                        if (product == null)
                        {

                            continue;
                        }

                        basketItems.Add(new BasketCookieVM()
                        {
                            Id = item.Id,
                            Count = item.Count
                        });
                    }
                }

                Response.Cookies.Append("Basket", JsonConvert.SerializeObject(basketCookies));

            }
            return View(basketItems);


        }
        public async Task<IActionResult> AddBasket(int id)
        {
            if (id <= 0) return BadRequest();
            Product book = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);

            if (book is null) return NotFound();

            List<BasketItemVM> prdItemVMs = new List<BasketItemVM>();



            if (User.Identity.IsAuthenticated)
            {
                AppUser user = await _userManager.Users
                    .Include(x => x.BasketItems)
                    .FirstOrDefaultAsync(x => x.Id == User.FindFirstValue(ClaimTypes.NameIdentifier));

                BasketItem basketItem = user.BasketItems.FirstOrDefault(x => x.ProductId == id);

                if (basketItem is not null)
                {
                    basketItem.Count++;
                }
                else
                {
                    user.BasketItems.Add(new BasketItem
                    {
                        Count = 1,
                        ProductId = id
                    });
                }

                await _context.SaveChangesAsync();

                foreach (var dbitem in user.BasketItems)
                {
                    Product newprd = await _context.Products
                        .Include(x => x.ProductImages.Where(x => x.IsPrime == true))
                        .FirstOrDefaultAsync(x => x.Id == dbitem.ProductId);

                    if (newprd is not null)
                    {
                        prdItemVMs.Add(new BasketItemVM
                        {
                            Id = newprd.Id,
                            Name = newprd.Name,
                            Price = newprd.Price,
                            ImgUrl = newprd.ProductImages[0].ImageUrl,
                            Count = dbitem.Count
                        });
                    }
                }


            }
            else
            {
                List<BasketCookieVM> basket = new List<BasketCookieVM>();

                BasketCookieVM cookieItem = null;

                string json = Request.Cookies["Books"];

                if (json is not null)
                {
                    basket = JsonConvert.DeserializeObject<List<BasketCookieVM>>(json);
                    cookieItem = basket.FirstOrDefault(x => x.Id == id);
                }

                if (cookieItem is not null)
                {
                    cookieItem.Count++;
                }
                else
                {
                    basket.Add(new BasketCookieVM
                    {
                        Id = id,
                        Count = 1
                    });
                }

                Response.Cookies.Append("Products", JsonConvert.SerializeObject(basket));



                foreach (var item in basket)
                {
                    Product newprd = await _context.Products
                        .Include(x => x.ProductImages.Where(x => x.IsPrime == true))
                        .FirstOrDefaultAsync(x => x.Id == item.Id);

                    if (newprd is not null)
                    {
                        prdItemVMs.Add(new BasketItemVM
                        {
                            Id = item.Id,
                            Name = newprd.Name,
                            Price = newprd.Price,
                            ImgUrl = newprd.ProductImages[0].ImageUrl,
                            Count = item.Count
                        });
                    }
                }
            }


            return PartialView("_BasketPartialView", prdItemVMs);

        }

        public IActionResult RemoveBasket(int id)
        {
            string json = Request.Cookies["Basket"];
            if(json != null)
            {
                List<BasketCookieVM> basket = JsonConvert.DeserializeObject<List<BasketCookieVM>>(json);
               BasketCookieVM product=basket.FirstOrDefault(p => p.Id == id);
                if(product != null)
                {
                    basket.Remove(product);
                }
                Response.Cookies.Append("Basket", JsonConvert.SerializeObject(basket));
            }


            return RedirectToAction("Index", "Home");
        }
    }
}
