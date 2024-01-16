using Amado.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Amado.Controllers
{
    public class AdminController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AdminController(RoleManager<IdentityRole> roleManager,UserManager<AppUser> userManager,SignInManager<AppUser> signInManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Register()
        {
            AppUser appUser = new AppUser
            {
                Name = "admin",
                UserName = "admin",
                Email = "admin@mail.com"
            };

            IdentityResult identityResult = await _userManager.CreateAsync(appUser, "admin");

            if(!identityResult.Succeeded)
            {
                foreach (var item in identityResult.Errors)
                {
                    ModelState.AddModelError("", "error");
                }
            }

            await _userManager.AddToRoleAsync(appUser, "Admin");

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> CreateRole()
        {
            await _roleManager.CreateAsync(new IdentityRole { Name = "Admin" });

            return Ok();
        }
    }
}
