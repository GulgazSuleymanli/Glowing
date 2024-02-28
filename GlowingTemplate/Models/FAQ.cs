using GlowingTemplate.Models.Entity;

namespace GlowingTemplate.Models
{
    public class FAQ:BaseEntity
    {
        public string Question { get; set; }
        public string  Answer { get; set; }
    }
}
