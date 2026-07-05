using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcProject.Data;
using MvcProject.Models;

namespace MvcProject.Controllers
{
    public class UsersController : Controller
    {
        private readonly MvcDbContext _context;

        public UsersController(MvcDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var mvcDbContext = _context.Users.Include(u => u.Role);
            return View(await mvcDbContext.ToListAsync());
        }

        public IActionResult Create()
        {
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(User user, string password)
        {
            ModelState.Remove("Password");
            ModelState.Remove("Role");
            
            if (ModelState.IsValid)
            {
                user.Password = password;
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Name", user.RoleId);
            return View(user);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();
            
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Name", user.RoleId);
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(User user, string? newPassword)
        {
            ModelState.Remove("Password");
            ModelState.Remove("Role");
            
            if (ModelState.IsValid)
            {
                var existingUser = await _context.Users.FindAsync(user.Id);
                if (existingUser != null)
                {
                    existingUser.FirstName = user.FirstName;
                    existingUser.LastName = user.LastName;
                    existingUser.Email = user.Email;
                    existingUser.RoleId = user.RoleId;

                    if (!string.IsNullOrEmpty(newPassword))
                    {
                        existingUser.Password = newPassword;
                    }

                    _context.Update(existingUser);
                    await _context.SaveChangesAsync();
                }
                
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Name", user.RoleId);
            return View(user);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
