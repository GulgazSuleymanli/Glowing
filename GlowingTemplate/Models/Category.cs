using GlowingTemplate.Models.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace GlowingTemplate.Models
{
    public class Category:BaseEntity
    {

        public string Name { get; set; }
        public string? ImageUrl { get; set; }
        [NotMapped]
        public IFormFile? ImageFile { get; set; }
        public List<Product>? Products { get; set; }
    }
}
