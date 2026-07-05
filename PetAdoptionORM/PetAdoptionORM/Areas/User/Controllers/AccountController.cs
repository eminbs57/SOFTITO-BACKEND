using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PetAdoptionORM.Model;
using System.Threading.Tasks;

namespace PetAdoptionORM.Areas.User.Controllers
{
    [Area("User")]
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

        // --- USER LOGIN (DEFAULT) ---
        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Admin")) return RedirectToAction("Index", "Pet", new { area = "Admin" });
                return RedirectToAction("Index", "Home", new { area = "User" });
            }
            ViewBag.IsAdminLogin = false;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                    if (result.Succeeded)
                    {
                        var roles = await _userManager.GetRolesAsync(user);
                        if (roles.Contains("Admin"))
                            return RedirectToAction("Index", "Home", new { area = "Admin" });
                        else
                            return RedirectToAction("Index", "Home", new { area = "User" });
                    }
                }
                ModelState.AddModelError("", "Hatalı e-posta veya şifre!");
            }
            ViewBag.IsAdminLogin = false;
            return View(model);
        }

        // --- ADMIN LOGIN ---
        [HttpGet]
        public IActionResult AdminLogin()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Admin")) return RedirectToAction("Index", "Pet", new { area = "Admin" });
                return RedirectToAction("Index", "Home", new { area = "User" });
            }
            ViewBag.IsAdminLogin = true;
            return View("Login"); // Re-use the same view, just flip the flag
        }

        [HttpPost]
        public async Task<IActionResult> AdminLogin(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    if (roles.Contains("Admin"))
                    {
                        var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                        if (result.Succeeded)
                        {
                            return RedirectToAction("Index", "Home", new { area = "Admin" });
                        }
                    }
                }
                ModelState.AddModelError("", "Hatalı yönetici bilgileri veya yetkisiz erişim!");
            }
            ViewBag.IsAdminLogin = true;
            return View("Login", model);
        }

        // --- REGISTER ---
        [HttpGet]
        public IActionResult Register(bool isAdmin = false)
        {
            var model = new RegisterViewModel { IsAdmin = isAdmin };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
            {
                ModelState.AddModelError("", "Lütfen email ve şifre alanlarını doldurun.");
                return View(model);
            }

            if (model.Password != model.ConfirmPassword)
            {
                ModelState.AddModelError("", "Girdiğiniz şifreler uyuşmuyor.");
                return View(model);
            }

            var user = new AppUser
            {
                UserName = model.Email,
                Email = model.Email,
                FullName = model.FullName ?? "Bilinmiyor",
                Address = model.Address ?? ""
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                // Create roles if they don't exist
                string roleName = model.IsAdmin ? "Admin" : "User";
                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    await _roleManager.CreateAsync(new IdentityRole(roleName));
                }

                // Assign role
                await _userManager.AddToRoleAsync(user, roleName);

                TempData["SuccessMessage"] = "Kayıt işlemi başarıyla tamamlandı! Lütfen giriş yapınız.";

                // We will redirect to login instead of signing in directly, to ensure they see the success message
                if (model.IsAdmin)
                    return RedirectToAction("AdminLogin", "Account", new { area = "User" });
                else
                    return RedirectToAction("Login", "Account", new { area = "User" });
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account", new { area = "User" });
        }
        
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
