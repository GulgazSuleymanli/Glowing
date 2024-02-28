using GlowingTemplate.Models.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace GlowingTemplate.Models
{
    public class Section:BaseEntity
    {       
        public string Title { get; set; }
        public string Description { get; set; }
        public string? ImageUrl { get; set; }
        [NotMapped]
        public IFormFile? ImageFile { get; set; }
    }
}
