using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBlog.Models
{
    [Table("ApplicationUsers")]
    public class ApplicationUser:IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set;}

        //relation
        public  List<Post> Posts { get; set; }
    }
}
