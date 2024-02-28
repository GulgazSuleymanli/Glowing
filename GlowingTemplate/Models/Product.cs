using GlowingTemplate.Models.Entity;

namespace GlowingTemplate.Models
{
    public class Product:BaseEntity
    {
       
        public string  Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string SKU { get; set;}
        public int Size { get; set; }
        public int CategoryId { get; set; }
        public bool IsDeleted { get; set; }

        public Category Category { get; set; }
        public List<ProductImage> ProductImages { get; set; }
        public List<BasketItem> BasketItems { get; set; }
        

    }
}
