using WebBlog.Models;
using X.PagedList;

namespace WebBlog.ViewModels
{
    public class HomeVM
    {
        public string? Title { get; set; }
        public string? ShortDescription { get; set; }
        public string? ImageUrl { get; set; }
        public List<Post>? Posts { get; set; }
    }
}
