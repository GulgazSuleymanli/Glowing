using GlowingTemplate.Models.Entity;

namespace GlowingTemplate.Models
{
    public class Setting : BaseEntity
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
