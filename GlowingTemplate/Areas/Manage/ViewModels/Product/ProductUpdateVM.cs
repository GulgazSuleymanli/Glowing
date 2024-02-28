using GlowingTemplate.Models;
using System.ComponentModel.DataAnnotations;

namespace GlowingTemplate.Areas.Manage.ViewModels.Product
{
    public class ProductUpdateVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string SKU { get; set; }
        public int CategoryId { get; set; }
        public int Size { get; set; }

        public List<Category>? Categories { get; set; }
        public IFormFile? MainPhoto { get; set; }
        public IFormFile? HoverPhoto { get; set; }
        public List<IFormFile>? Photo { get; set; }

        public List<ProductImageVM> ProductImagesVM { get; set; }
        public List<int>? ImageIds { get; set; }
    }


    public class ProductImageVM
    {
        public int Id { get; set; }
        public string ImgUrl { get; set; }
        public bool? IsPrime { get; set; }
    }

}
