using GlowingTemplate.Models.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GlowingTemplate.Models
{
    public class Slider:BaseEntity
    {
        [MaxLength(15)]
        public string Title { get; set; }
        [MaxLength(50)]
        public string Description { get; set; }
        public string? ImageUrl { get; set; }
        [NotMapped]
        public IFormFile? ImageFile { get; set; }

    }
}
