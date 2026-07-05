using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcProject.Data;
using MvcProject.Models;

namespace MvcProject.Controllers
{
    public class RolesController : Controller
    {
        private readonly MvcDbContext _context;

        public RolesController(MvcDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var mvcDbContext = _context.Roles.Include(r => r.Department);
            return View(await mvcDbContext.ToListAsync());
        }

        public IActionResult Create()
        {
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Role role)
        {
            ModelState.Remove("Department");
            if (ModelState.IsValid)
            {
                _context.Add(role);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name", role.DepartmentId);
            return View(role);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var role = await _context.Roles.FindAsync(id);
            if (role == null) return NotFound();
            
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name", role.DepartmentId);
            return View(role);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Role role)
        {
            ModelState.Remove("Department");
            if (ModelState.IsValid)
            {
                _context.Update(role);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name", role.DepartmentId);
            return View(role);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role != null)
            {
                _context.Roles.Remove(role);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
