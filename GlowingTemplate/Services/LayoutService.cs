using GlowingTemplate.DAL;
using GlowingTemplate.Models;
using GlowingTemplate.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;

namespace GlowingTemplate.Services
{
    public class LayoutService
    {
        AppDbContext _appDbContext;
        private readonly IHttpContextAccessor _http;
        private readonly UserManager<AppUser> _userManager;

        public LayoutService(AppDbContext appDbContext, IHttpContextAccessor http, UserManager<AppUser> userManager)
        {
            _appDbContext = appDbContext;
            _http = http;
            _userManager = userManager;
        }

        public async Task<Dictionary<string, string>> GetSetting()
        {
            var setting = await _appDbContext.Settings.ToDictionaryAsync(s => s.Key, s => s.Value);
            return setting;
        }
        public async Task<List<BasketItemVM>> GetBasket()
        {
            List<BasketItemVM> bookItemVMs = new List<BasketItemVM>();

            if (_http.HttpContext.User.Identity.IsAuthenticated)
            {
                AppUser user = await _userManager.Users
                   .Include(x => x.BasketItems)
                   .ThenInclude(x => x.Product)
                   .ThenInclude(y => y.ProductImages.Where(x => x.IsPrime == true))
                   .FirstOrDefaultAsync(x => x.Id == _http.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

                foreach (var dbitem in user.BasketItems)
                {
                    Product newprd = await _appDbContext.Products
                        .Include(x => x.ProductImages.Where(x => x.IsPrime == true))
                        .FirstOrDefaultAsync(x => x.Id == dbitem.ProductId);

                    if (newprd is not null)
                    {
                        bookItemVMs.Add(new BasketItemVM
                        {
                            Id = newprd.Id,
                            Name = newprd.Name,
                            
                            ImgUrl = newprd.ProductImages[0].ImageUrl,
                            Subtotal = newprd.Price * dbitem.Count,
                            Count = dbitem.Count
                        });
                    }
                }
            }
            else
            {
                string json = _http.HttpContext.Request.Cookies["Books"];

                if (json != null)
                {
                    List<BasketCookieVM> basket = JsonConvert.DeserializeObject<List<BasketCookieVM>>(json);

                    foreach (var item in basket)
                    {
                        Product prd = await _appDbContext.Products
                            .Include(x => x.ProductImages.Where(x => x.IsPrime == true))
                            .FirstOrDefaultAsync(x => x.Id == item.Id);

                        if (prd is not null)
                        {
                            bookItemVMs.Add(new BasketItemVM
                            {
                                Id = item.Id,
                                Name = prd.Name,
                                Price = prd.Price,
                                ImgUrl = prd.ProductImages[0].ImageUrl,
                                Subtotal = prd.Price * item.Count,
                                Count = item.Count
                            });
                        }
                    }

                }
            }

            return bookItemVMs;


        }

    }
}
