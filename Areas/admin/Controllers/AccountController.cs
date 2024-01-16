using Amado.Areas.admin.ViewModels.Account;
using Amado.Interface;
using Amado.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Amado.Areas.admin.Controllers
{
    [Area("admin")]
    public class AccountController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IEmailRepository _emailRepository;

        public AccountController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,IEmailRepository emailRepository)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailRepository = emailRepository;
        }

        [HttpGet]

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid) return View(loginVM);

            AppUser appUser = await _userManager.FindByNameAsync(loginVM.UserName);

            if (appUser == null)
            {
                ModelState.AddModelError("", "Username or Password is incorrect");
                return View(loginVM);
            }

            Microsoft.AspNetCore.Identity.SignInResult signInResult = await _signInManager.CheckPasswordSignInAsync(appUser, loginVM.Password, true);


            if (!signInResult.Succeeded)
            {
                ModelState.AddModelError("", "Username or Password is incorrect");
                return View(loginVM);
            }

            await _signInManager.PasswordSignInAsync(appUser, loginVM.Password, loginVM.RemindMe, true);

            return RedirectToAction("Index", "Product", new { area = "admin" });
        }

        //public async Task<IActionResult> Register()
        //{
        //    AppUser appUser = new AppUser
        //    {
        //        Name = "admin",
        //        UserName = "admin",
        //        Email = "admin@mail.com"
        //    };

        //    IdentityResult identityResult = await _userManager.CreateAsync(appUser, "admin");

        //    if (!identityResult.Succeeded)
        //    {
        //        foreach (var item in identityResult.Errors)
        //        {
        //            ModelState.AddModelError("", "error");
        //        }
        //    }

        //    await _userManager.AddToRoleAsync(appUser, "Admin");

        //    return RedirectToAction("Index", "Home");
        //}

        public async Task<IActionResult> CreateRole()
        {
            await _roleManager.CreateAsync(new IdentityRole { Name = "Admin" });

            return Ok();
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Login", "Account");
        }
    }
}
