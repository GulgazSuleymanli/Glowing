namespace GlowingTemplate.Areas.Manage.ViewModels
{
    public class PaginateVM<T> where T : class, new()
    {
        public int Take { get; set; }
        public decimal TotalPage { get; set; }
        public int CurrentPage { get; set; }
        public List<T> Items { get; set; }
    }
}
