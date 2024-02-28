using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBlog.Data;
using WebBlog.ViewModels;

namespace WebBlog.Controllers
{
    public class PageController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PageController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> About()
        {
            var page = await _context.Pages!.FirstOrDefaultAsync(x => x.Slug == "About");
            var vm = new PageVM()
            {
                Title = page!.Title,
                ShortDescription = page.ShortDescription,
                Description = page.Description,
                ImageUrl = page.ImageUrl,
            };
            return View(vm);
        }
        public async Task<IActionResult> Contact()
        {
            var page = await _context.Pages!.FirstOrDefaultAsync(x => x.Slug == "Contact");
            var vm = new PageVM()
            {
                Title = page!.Title,
                ShortDescription = page.ShortDescription,
                Description = page.Description,
                ImageUrl = page.ImageUrl,
            };
            return View(vm);
        }
        public async Task<IActionResult> PrivacyPolicy()
        {
            var page = await _context.Pages!.FirstOrDefaultAsync(x => x.Slug == "PrivacyPolicy");
            var vm = new PageVM()
            {
                Title = page!.Title,
                ShortDescription = page.ShortDescription,
                Description = page.Description,
                ImageUrl = page.ImageUrl,
            };
            return View(vm);
        }
    }
}
