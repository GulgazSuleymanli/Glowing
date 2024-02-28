using GlowingTemplate.Models;
using System.ComponentModel.DataAnnotations;

namespace GlowingTemplate.Areas.Manage.ViewModels
{
    public class ProductCreateVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string SKU { get; set; }
        public int CategoryId { get; set; }
        public int Size { get; set; }

        public List<Category>? Categories { get; set; }
       
        [Required]
        public IFormFile MainPhoto { get; set; }
        [Required]
        public IFormFile HoverPhoto { get; set; }
        public List<IFormFile>? Photo { get; set; }


    }
}
