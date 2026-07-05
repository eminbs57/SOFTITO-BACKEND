using Microsoft.AspNetCore.Mvc;
using EventManagement.Data;
using EventManagement.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;

namespace EventManagement.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public UserController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index() => View();

        public async Task<JsonResult> UserList()
        {
            var users = _userManager.Users.ToList();
            var userList = new List<object>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userList.Add(new
                {
                    id = user.Id,
                    username = user.UserName,
                    role = roles.FirstOrDefault() ?? "User"
                });
            }
            return new JsonResult(userList);
        }

        [HttpPost]
        public async Task<JsonResult> AddUser(string username, string password, string role)
        {
            var user = new AppUser { UserName = username };
            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(role))
                {
                    await _userManager.AddToRoleAsync(user, role);
                }
                else
                {
                    await _userManager.AddToRoleAsync(user, "User");
                }
                return new JsonResult("Başarıyla kaydedildi");
            }
            return new JsonResult("Kayıt başarısız: " + string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        public async Task<JsonResult> Edit(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user != null)
            {
                var roles = await _userManager.GetRolesAsync(user);
                return new JsonResult(new
                {
                    id = user.Id,
                    username = user.UserName,
                    role = roles.FirstOrDefault() ?? "User"
                });
            }
            return new JsonResult(null);
        }

        [HttpPost]
        public async Task<JsonResult> Update(int id, string username, string role)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user != null)
            {
                user.UserName = username;
                await _userManager.UpdateAsync(user);

                var currentRoles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, currentRoles);
                await _userManager.AddToRoleAsync(user, role ?? "User");

                return new JsonResult("Kayıt güncellendi");
            }
            return new JsonResult("Kullanıcı bulunamadı");
        }

        public async Task<JsonResult> Delete(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
                return new JsonResult("Kayıt silindi");
            }
            return new JsonResult("Kullanıcı bulunamadı");
        }

        public async Task<JsonResult> SearchUser(string searchString)
        {
            var users = _userManager.Users.ToList();
            var userList = new List<object>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var roleName = roles.FirstOrDefault() ?? "User";
                
                if (string.IsNullOrEmpty(searchString) || 
                    user.UserName.Contains(searchString, StringComparison.OrdinalIgnoreCase) || 
                    roleName.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                {
                    userList.Add(new
                    {
                        id = user.Id,
                        username = user.UserName,
                        role = roleName
                    });
                }
            }
            return new JsonResult(userList);
        }
    }
}