using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBlog.Data;
using WebBlog.ViewModels;

namespace WebBlog.Controllers
{
    public class BlogController : Controller
    {
        private readonly ApplicationDbContext _context;
        private INotyfService _notification { get; }

        public BlogController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Post(string slug)
        {
            if (slug == null)
            {
                _notification.Error("Bài viết lỗi");
                return View();
            }
            var post = _context.Posts!.Include(x=>x.ApplicationUser).FirstOrDefault(x=>x.Slug == slug);
            if (post == null)
            {
                _notification.Error("Không có bài viết");
                return View();
            }
            var vm = new BlogPostVM()
            {
                Id = post.Id,
                Title = post.Title,
                AuthorName = post.ApplicationUser!.FirstName + " " + post.ApplicationUser.LastName,
                CreateDate = post.CreatedDate,
                ImageUrl = post.ImageUrl,
                Description = post.Description,
                ShortDescription = post.ShortDescription,
            };
            return View(vm);
        }
    }
}
