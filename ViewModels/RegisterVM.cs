using Microsoft.Build.Framework;

namespace WebBlog.ViewModels
{
    public class RegisterVM
    {
        [Required]
        public string? UserName { get; set; }
        [Required]
        public string? Password { get; set; }
        [Required]
        public string? RePassword { get; set; }
    }
}
