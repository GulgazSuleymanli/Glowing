using GlowingTemplate.DAL;
using GlowingTemplate.Models;
using GlowingTemplate.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace GlowingTemplate.Services
{
    public class CustemService
    {
        AppDbContext _appDbContext;
        private readonly IHttpContextAccessor _http;
        private readonly UserManager<AppUser> _userManager;

        public CustemService(AppDbContext appDbContext, IHttpContextAccessor http, UserManager<AppUser> userManager)
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
            List<BasketItemVM> basket = new List<BasketItemVM>();

            if (_http.HttpContext.User.Identity.IsAuthenticated)
            {
                AppUser user = await _userManager.FindByNameAsync(_http.HttpContext.User.Identity.Name);
                List<BasketItem> userBasket = await _appDbContext.BasketItems
                    .Where(b => b.AppUserId == user.Id && b.OrderId == null)
                    .Include(b => b.Product)
                    .ThenInclude(p => p.ProductImages
                    .Where(pi => pi.IsPrime == true)).ToListAsync();
                foreach (var item in userBasket)
                {
                    basket.Add(new BasketItemVM()
                    {
                        Price = item.Price,
                        Count = item.Count,
                        ImgUrl = item.Product.ProductImages.FirstOrDefault().ImageUrl,
                        Name = item.Product.Name
                    });
                }

            }
            else
            {

                var jsonBasket = _http.HttpContext.Request.Cookies["Basket"];
                if (jsonBasket != null)
                {
                    List<BasketItemVM> basketCookie = JsonConvert.DeserializeObject<List<BasketItemVM>>(jsonBasket);

                    foreach (var item in basketCookie)
                    {
                        Product product = _appDbContext.Products.Where(p => p.IsDeleted == false).Include(p => p.ProductImages.Where(pi => pi.IsPrime == true)).FirstOrDefault(p => p.Id == item.Id);

                        if (product == null)
                        {

                            continue;
                        }

                        basket.Add(new BasketItemVM()
                        {
                            Id = item.Id,
                            Name = product.Name,
                            Price = product.Price,
                            Count = item.Count,
                            ImgUrl = product.ProductImages.FirstOrDefault(pi => pi.IsPrime == true).ImageUrl
                        });
                    }
                }
            }
            return basket;


        }
    }
}
