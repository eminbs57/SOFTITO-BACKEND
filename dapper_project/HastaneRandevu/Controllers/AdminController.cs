using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Dapper;
using HastaneRandevu.Models;

namespace HastaneRandevu.Controllers
{
    public class AdminController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(AdminModel model)
        {
            if (string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.PasswordHash))
            {
                ViewBag.Error = "Kullanıcı adı ve şifre boş olamaz.";
                return View();
            }

            DynamicParameters param = new DynamicParameters();
            param.Add("@Username", model.Username);
            param.Add("@PasswordHash", model.PasswordHash); // In a real app, hash this before sending to DB

            var admin = Context.Listeleme<AdminModel>("AdminLogin", param).FirstOrDefault();

            if (admin != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, admin.Username),
                    new Claim(ClaimTypes.NameIdentifier, admin.AdminID.ToString())
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties { IsPersistent = true };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Geçersiz kullanıcı adı veya şifre.";
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(AdminModel model)
        {
             if (string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.PasswordHash))
             {
                 ViewBag.Error = "Kullanıcı adı ve şifre boş olamaz.";
                 return View();
             }

             try
             {
                 DynamicParameters param = new DynamicParameters();
                 param.Add("@Username", model.Username);
                 param.Add("@PasswordHash", model.PasswordHash);
                 
                 Context.ExecuteReturn("AdminRegister", param);
                 return RedirectToAction("Login");
             }
             catch(Exception ex)
             {
                 ViewBag.Error = "Kayıt sırasında bir hata oluştu (Kullanıcı adı kullanılıyor olabilir). Detay: " + ex.Message;
                 return View();
             }
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}
