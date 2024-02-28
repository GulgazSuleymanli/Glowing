using GlowingTemplate.Models.Entity;

namespace GlowingTemplate.Models
{
    public class ProductImage:BaseEntity
    {
      
        public bool? IsPrime { get; set; }
        public string ImageUrl { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
