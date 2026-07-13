using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ObiletApp.Core.Entities;
using System.Threading.Tasks;

namespace ObiletApp.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    if (user != null)
                    {
                        var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, false, false);
                        if (result.Succeeded)
                        {
                            var roles = await _userManager.GetRolesAsync(user);
                            if (roles.Contains("Admin"))
                            {
                                return RedirectToAction("Index", "Admin");
                            }
                            return RedirectToLocal(returnUrl);
                        }
                    }
                    ModelState.AddModelError(string.Empty, "Geçersiz giriş denemesi. Lütfen bilgilerinizi kontrol edin.");
                }
                catch (System.Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Veritabanı bağlantı hatası oluştu. Lütfen sunucu ayarlarınızı kontrol edin.");
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = new AppUser { UserName = model.Email, Email = model.Email, FirstName = model.FirstName, LastName = model.LastName };
                    var result = await _userManager.CreateAsync(user, model.Password);

                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, "User");
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return RedirectToAction("Index", "Home");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                catch (System.Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Kayıt işlemi sırasında bir hata oluştu. Veritabanı bağlantınızı kontrol edin.");
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult AdminRegister()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AdminRegister(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = new AppUser { UserName = model.Email, Email = model.Email, FirstName = model.FirstName, LastName = model.LastName };
                    var result = await _userManager.CreateAsync(user, model.Password);

                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, "Admin");
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return RedirectToAction("Index", "Admin");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                catch (System.Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Yönetici kayıt işlemi başarısız. Veritabanı tablolara erişemiyor olabilir.");
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }
    }

    public class LoginViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class RegisterViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
