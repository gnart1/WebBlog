using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using WebBlog.Data;
using WebBlog.Models;
using WebBlog.ViewModels;

namespace WebBlog.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin")]
    public class PageController : Controller
    {
        private readonly ApplicationDbContext _context;
        public INotyfService _notification { get; }
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PageController(ApplicationDbContext context, INotyfService notification, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _notification = notification;
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpGet]
        public async Task<IActionResult> About()
        {
            var page = await _context.Pages!.FirstOrDefaultAsync(x => x.Slug == "About");
            var vm = new PageVM()
            {
                Id = page!.Id,
                Title = page.Title,
                ShortDescription = page.ShortDescription,
                Description = page.Description,
                ImageUrl = page.ImageUrl,

            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> About(PageVM vm)
        {
            if(!ModelState.IsValid)
            {
                return View(vm);
            }
            var page = await _context.Pages!.FirstOrDefaultAsync(x => x.Slug == "About");
            if(page == null)
            {
                _notification.Error("Không có thông tin");
                return View();
            }
            page.Title = vm.Title;
            page.ShortDescription = vm.ShortDescription;
            page.Description = vm.Description;
            if (vm.Image != null)
            {
                page.ImageUrl = UploadImage(vm.Image!);
            }
            //_context.Posts!.AddAsync(aboutPage);
            _context.Pages!.Update(page);
            await _context.SaveChangesAsync();
            _notification.Success("Sửa trang thành công!");
            return RedirectToAction("About", "Page", new { area = "Admin"});
        }

        [HttpGet]
        public async Task<IActionResult> Contact()
        {
            var contactPage = await _context.Pages!.FirstOrDefaultAsync(x => x.Slug == "Contact");
            var vm = new PageVM()
            {
                Id = contactPage!.Id,
                Title = contactPage.Title,
                ShortDescription = contactPage.ShortDescription,
                Description = contactPage.Description,
                ImageUrl = contactPage.ImageUrl,

            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Contact(PageVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var page = await _context.Pages!.FirstOrDefaultAsync(x => x.Slug == "Contact");
            if (page == null)
            {
                _notification.Error("Không có thông tin");
                return View();
            }
            page.Title = vm.Title;
            page.ShortDescription = vm.ShortDescription;
            page.Description = vm.Description;
            if (vm.Image != null)
            {
                page.ImageUrl = UploadImage(vm.Image);
            }
            //_context.Posts!.AddAsync(aboutPage);
            await _context.SaveChangesAsync();
            _notification.Success("Sửa liên hệ thành công!");
            return RedirectToAction("Contact", "Page", new { area = "Admin" });
        }

        [HttpGet]
        public async Task<IActionResult> PrivacyPolicy()
        {
            var privacyPage = await _context.Pages!.FirstOrDefaultAsync(x => x.Slug == "PrivacyPolicy");
            var vm = new PageVM()
            {
                Id = privacyPage!.Id,
                Title = privacyPage.Title,
                ShortDescription = privacyPage.ShortDescription,
                Description = privacyPage.Description,
                ImageUrl = privacyPage.ImageUrl,

            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> PrivacyPolicy(PageVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var page = await _context.Pages!.FirstOrDefaultAsync(x => x.Slug == "PrivacyPolicy");
            if (page == null)
            {
                _notification.Error("Không có thông tin");
                return View();
            }
            page.Title = vm.Title;
            page.ShortDescription = vm.ShortDescription;
            page.Description = vm.Description;
            if (vm.Image != null)
            {
                page.ImageUrl = UploadImage(vm.Image);
            }
            //_context.Posts!.AddAsync(aboutPage);
            await _context.SaveChangesAsync();
            _notification.Success("Sửa bảo mật thành công!");
            return RedirectToAction("PrivacyPolicy", "Page", new { area = "Admin" });
        }
        private string UploadImage(IFormFile file)
        {
            string uniqueFileName = "";
            var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "image");
            uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            var filePath = Path.Combine(folderPath, uniqueFileName);
            using (FileStream fileStream = System.IO.File.Create(filePath))
            {
                file.CopyTo(fileStream);
            }
            return uniqueFileName;
        }
    }
}
