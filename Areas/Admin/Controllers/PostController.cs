using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using WebBlog.Data;
using WebBlog.Models;
using WebBlog.Utilites;
using WebBlog.ViewModels;
using X.PagedList;

namespace WebBlog.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class PostController : Controller
    {
        private readonly ApplicationDbContext _context;
        public INotyfService _notification { get; }
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;

        public PostController(ApplicationDbContext context, INotyfService notification,
            IWebHostEnvironment webHostEnvironment, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _notification = notification;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? page)
        {
            var listOfPosts = new List<Post>();
            var loggedInUser = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == User.Identity!.Name);
            var loggedInUserRole = await _userManager.GetRolesAsync(loggedInUser!);
            if (loggedInUserRole[0] == Roles.WebAdmin)
            {
                listOfPosts = await _context.Posts!.Include(x => x.ApplicationUser).ToListAsync();
            }
            else
            {
                listOfPosts = await _context.Posts!.Include(x => x.ApplicationUser).Where(x=>x.ApplicationUser!.Id == loggedInUser!.Id).ToListAsync();
            }


            var listOfPostsVM = listOfPosts.Select(x => new PostVM()
            {
                Id = x.Id,
                Title = x.Title,
                CreateDate = x.CreatedDate,
                ImageUrl = x.ImageUrl,
                AuthorName = x.ApplicationUser!.FirstName + " " + x.ApplicationUser.LastName
            }).ToList();

            int pageSize = 3;
            int pageNumber = (page ?? 1);

            return View(await listOfPostsVM.ToPagedListAsync(pageNumber, pageSize));
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new CreatePostVM());
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreatePostVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var loggedInUser = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == User.Identity!.Name);

            var post = new Post();
            post.Title = vm.Title;
            post.Description = vm.Description;
            post.ShortDescription = vm.ShortDescription;
            post.ApplicationUserId = loggedInUser!.Id;
            if(post.Title != null) 
            {
                string slug = vm.Title!.Trim();
                slug = slug.Replace(" ", "-");
                post.Slug = slug + "-" + Guid.NewGuid();
            }

            if(vm.Image != null)
            {
               post.ImageUrl = UploadImage(vm.Image);
            }
            await _context.Posts!.AddAsync(post);
            await _context.SaveChangesAsync();
            _notification.Success("Tạo bài viết thành công!");
            return RedirectToAction("Index");
            
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var post = await _context.Posts!.FirstOrDefaultAsync(x=> x.Id == id);
            if (post == null)
            {
                _notification.Error("Error");
                return View();
            }

            var loggedInUser = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == User.Identity!.Name);
            var loggedInUserRole = await _userManager.GetRolesAsync(loggedInUser!);
            if (loggedInUserRole[0] != Roles.WebAdmin && loggedInUser!.Id != post.ApplicationUserId)
            {
                _notification.Error("Bạn không có quyền sửa bài viết của người khác");
                return RedirectToAction("Index");
            }
            
            var vm = new CreatePostVM()
            {
                Id = post.Id,
                Title = post.Title,
                ShortDescription = post.ShortDescription,
                Description = post.Description,
                ImageUrl = post.ImageUrl,
            };
            return View(vm);
        }
        public async Task<IActionResult> Edit(CreatePostVM vm)
        {
            if(!ModelState.IsValid)
            {
                return View(vm);
            }
            var post = await _context.Posts!.FirstOrDefaultAsync(x=>x.Id == vm.Id);
            if(post == null)
            {
                _notification.Error("Error");
                return View();
            }
            post.Title = vm.Title;
            post.ShortDescription = vm.ShortDescription;
            post.Description = vm.Description;
            if(vm.Image != null)
            {
                post.ImageUrl = UploadImage(vm.Image);
            }
            _context.Posts!.Update(post);
            await _context.SaveChangesAsync();
            _notification.Success("Sửa bài viết thành công!");
            return RedirectToAction("Index", "Post", new {area="Admin"});
        }
        private string UploadImage(IFormFile file)
        {
            string uniqueFileName = "";
            var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "image");
            uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            var filePath = Path.Combine(folderPath, uniqueFileName);
            using(FileStream fileStream = System.IO.File.Create(filePath))
            {
                file.CopyTo(fileStream);
            }
            return uniqueFileName;
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var post = await _context.Posts!.FirstOrDefaultAsync(x=>x.Id == id);

            var loggedInUser = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == User.Identity!.Name);
            var loggedInUserRole = await _userManager.GetRolesAsync(loggedInUser!);
            if (loggedInUserRole[0] == Roles.WebAdmin || loggedInUser?.Id == post?.ApplicationUserId)
            {
                _context.Posts!.Remove(post!);
                await _context.SaveChangesAsync();
                _notification.Success("Xóa bài viết thành công!");
                return RedirectToAction("Index", "Post", new { area = "Admin" });
            }
            if (post == null)
            {
                _notification.Error("Xóa bài viết không thành công!");
                return View();
            }
            return View();
            
        }
    }
}
