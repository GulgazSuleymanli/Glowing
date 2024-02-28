namespace GlowingTemplate.ViewModels
{
    public class BasketItemVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string ImgUrl { get; set; }
        public int Count { get; set; }
        public decimal Subtotal { get; set; }
    }
}
