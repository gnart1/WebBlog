using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBlog.Data;
using WebBlog.Models;
using WebBlog.ViewModels;

namespace WebBlog.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin")]
    public class SettingController : Controller
    {

        private readonly ApplicationDbContext _context;
        public INotyfService _notification { get; }
        private readonly IWebHostEnvironment _webHostEnvironment;

        public SettingController(ApplicationDbContext context, INotyfService notification, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _notification = notification;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var settings = _context.Settings!.ToList();
            if(settings.Count > 0)
            {
                var vm = new SettingVM()
                {
                    Id = settings[0].Id,
                    SiteName = settings[0].SiteName,
                    Title = settings[0].Title,
                    ShortDescription = settings[0].ShortDescription,
                    ImageUrl = settings[0].ImageUrl,
                    FacebookUrl = settings[0].FacebookUrl,
                    InstagramUrl = settings[0].InstagramUrl,
                    GithubUrl = settings[0].GithubUrl,
                };
                return View(vm);
            }
            var setting = new Setting()
            {
                SiteName = "Demo Name"
            };
            await _context.Settings!.AddAsync(setting);
            await _context.SaveChangesAsync();
            var createdSettings = _context.Settings!.ToList();
            var crearedVm = new SettingVM()
            {
                Id = createdSettings[0].Id,
                SiteName = createdSettings[0].SiteName,
                Title = createdSettings[0].Title,
                ShortDescription = createdSettings[0].ShortDescription,
                ImageUrl = createdSettings[0].ImageUrl,
                FacebookUrl = createdSettings[0].FacebookUrl,
                InstagramUrl = createdSettings[0].InstagramUrl,
                GithubUrl = createdSettings[0].GithubUrl,
            };
            return View(crearedVm);
        }
        [HttpPost]
        public async Task<IActionResult> Index(SettingVM vm)
        {
            if(!ModelState.IsValid)
            {
                return View(vm);
            }
            var setting = await _context.Settings!.FirstOrDefaultAsync(x=>x.Id == vm.Id);
            if(setting == null)
            {
                _notification.Error("không có cài đặt");
                return View(vm);
            }
            setting.SiteName = vm.SiteName;
            setting.Title = vm.Title;
            setting.ShortDescription = vm.ShortDescription;
            setting.FacebookUrl = vm.FacebookUrl;
            setting.InstagramUrl = vm.InstagramUrl;
            setting.GithubUrl = vm.GithubUrl;
            if(vm.Image != null)
            {
                setting.ImageUrl = UploadImage(vm.Image);
            }
            await _context.SaveChangesAsync();
            _notification.Success("Sửa cài đặt thành công!");

            return RedirectToAction("Index", "Setting", new { area = "Admin" });
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
