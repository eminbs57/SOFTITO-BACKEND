using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MvcProject.Data;
using MvcProject.Models;
using MvcProject.ViewModels;
using System.Security.Claims;

namespace MvcProject.Controllers
{
    public class AuthController : Controller
    {
        private readonly MvcDbContext _context;

        public AuthController(MvcDbContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users.FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password);
                if (user != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Email),
                        new Claim("FullName", user.FirstName + " " + user.LastName)
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme, 
                        new ClaimsPrincipal(claimsIdentity));

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "E-posta veya şifre hatalı");
                }
            }
            return View(model);
        }

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
                    if (_context.Users.Any(u => u.Email == model.Email))
                    {
                        ModelState.AddModelError("", "Bu e-posta adresi zaten kullanımda.");
                        return View(model);
                    }

                    // Varsayılan rol kontrolü (Yoksa oluştur)
                    var defaultRole = _context.Roles.FirstOrDefault();
                    if (defaultRole == null)
                    {
                        var defaultDept = new Department { Name = "Genel" };
                        _context.Departments.Add(defaultDept);
                        await _context.SaveChangesAsync();
                        
                        defaultRole = new Role { Name = "User", DepartmentId = defaultDept.Id };
                        _context.Roles.Add(defaultRole);
                        await _context.SaveChangesAsync();
                    }

                    User newUser = new User
                    {
                        FirstName = model.Name,
                        LastName = model.Name, 
                        Email = model.Email,
                        Password = model.Password,
                        RoleId = defaultRole.Id
                    };
                    
                    _context.Users.Add(newUser);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Login", "Auth");
                }
                catch (Exception ex)
                {
                    var innerMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    ModelState.AddModelError("", "Kayıt sırasında veritabanı hatası: " + innerMessage);
                }
            }
            return View(model);
        }

        public IActionResult ChangePassword(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("VerifyEmail", "Auth");
            }
            return View(new ChangePasswordViewModel { Email = username });
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users.FirstOrDefault(u => u.Email == model.Email);
                if (user != null)
                {
                    user.Password = model.NewPassword;
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Login", "Auth");
                }
                else
                {
                    ModelState.AddModelError("", "Email bulunamadı!");
                    return View(model);
                }
            }
            return View(model);
        }

        public IActionResult VerifyEmail()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> VerifyEmail(VerifyEmailViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users.FirstOrDefault(u => u.Email == model.Email);
                if (user == null)
                {
                    ModelState.AddModelError("", "Kayıtlı e-posta bulunamadı");
                    return View(model);
                }
                else
                {
                    return RedirectToAction("ChangePassword", "Auth", new { username = user.Email });
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToActionPermanent("Index", "Home");
        }
    }
}