using System.ComponentModel.DataAnnotations;

namespace GlowingTemplate.ViewModels.Account
{
    public class LoginVM
    {
        [Required]
        [MaxLength(25)]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }    
    }
}
