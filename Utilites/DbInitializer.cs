using Microsoft.AspNetCore.Identity;
using WebBlog.Data;
using WebBlog.Models;

namespace WebBlog.Utilites
{
    public class DbInitializer:IDbInitializer
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(ApplicationDbContext context, 
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void Initialize()
        {
            if (!_roleManager.RoleExistsAsync(Roles.WebAdmin).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(Roles.WebAdmin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(Roles.WebAuthor)).GetAwaiter().GetResult();
                _userManager.CreateAsync(new ApplicationUser()
                {
                    UserName = "admin@gmail.com",
                    Email = "admin@gmail.com",
                    FirstName = "Hà",
                    LastName = "Tráng",
                },"Admin@0011").Wait();

                var appUser = _context.ApplicationUsers.FirstOrDefault(x => x.Email == "admin@gmail.com");
                if(appUser != null)
                {
                    _userManager.AddToRoleAsync(appUser, Roles.WebAdmin).GetAwaiter().GetResult();
                }

                var listOfPages = new List<Page>()
                {
                    new Page()
                    {
                         Title = "About Us",
                    Slug = "About-Us"
                    },
                    new Page()
                    {
                        Title = "Contact Us",
                        Slug = "Contact"
                    },

                    new Page()
                    {
                        Title = "Privacy Policy",
                        Slug = "PrivacyPolicy"
                    }

                };
                _context.Pages.AddRange(listOfPages);
                _context.SaveChanges();
            }
        }
    }
}
