using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace dbfirstProjem.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Register(string role = "User")
        {
            ViewBag.Role = role;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string username, string password, string role)
        {
            ViewBag.Role = role;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(role))
            {
                ModelState.AddModelError("", "Tüm alanları doldurunuz.");
                return View();
            }

            if (!await _roleManager.RoleExistsAsync(role))
            {
                await _roleManager.CreateAsync(new IdentityRole(role));
            }

            var user = new IdentityUser { UserName = username, Email = username + "@example.com" };
            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, role);
                await _signInManager.SignInAsync(user, isPersistent: false);
                
                if (role == "Admin")
                {
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    return RedirectToAction("KullaniciEkrani", "Home");
                }
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password, string expectedRole)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                TempData["Error"] = "Kullanıcı adı veya şifre boş olamaz.";
                if (expectedRole == "Admin") return RedirectToAction("Login", "Admin");
                return RedirectToAction("Login", "Home");
            }

            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                TempData["Error"] = "Kullanıcı adı veya şifre hatalı.";
                if (expectedRole == "Admin") return RedirectToAction("Login", "Admin");
                return RedirectToAction("Login", "Home");
            }

            var roles = await _userManager.GetRolesAsync(user);
            
            // "user olarak kaydolan admin loginine, admin olarak kaydolan user girişine giremesin"
            if (expectedRole == "Admin" && !roles.Contains("Admin"))
            {
                TempData["Error"] = "Kullanıcı adı veya şifre hatalı.";
                return RedirectToAction("Login", "Admin");
            }

            if (expectedRole == "User" && !roles.Contains("User"))
            {
                TempData["Error"] = "Kullanıcı adı veya şifre hatalı.";
                return RedirectToAction("Login", "Home");
            }

            var result = await _signInManager.PasswordSignInAsync(username, password, false, lockoutOnFailure: false);
            
            if (result.Succeeded)
            {
                if (roles.Contains("Admin"))
                {
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    return RedirectToAction("KullaniciEkrani", "Home");
                }
            }
            
            TempData["Error"] = "Şifre hatalı.";
            if (expectedRole == "Admin") return RedirectToAction("Login", "Admin");
            return RedirectToAction("Login", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
