using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SpendSmart.Models;


//using Microsoft.AspNetCore.Mvc;
//using SpendSmart.Models;
//using Microsoft.EntityFrameworkCore;

namespace SpendSmart.Controllers
{
    public class UsersController : Controller
    {
        private readonly SpendSmartDbContext _context;

        public UsersController(SpendSmartDbContext context)
        {
            _context = context;
        }

        // Show all users
        public IActionResult Index()
        {
            var users = _context.User
                .Include(u => u.Expenses)
                .ToList();
            return View(users);
        }

        // Create/Edit form
        public IActionResult CreateEditUser(int? id)
        {
            if (id == null)
                return View(new User());

            var user = _context.User.Find(id);
            if (user == null)
                return NotFound();

            return View(user);
        }

        // Save user (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateEditUser(User model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (model.UserId == 0)
                _context.User.Add(model);
            else
                _context.User.Update(model);

            _context.SaveChanges();
            TempData["Success"] = "✅ User saved successfully!";
            return RedirectToAction(nameof(Index));
        }

        // Delete user
        public IActionResult DeleteUser(int id)
        {
            var user = _context.User
                .Include(u => u.Expenses)
                .FirstOrDefault(u => u.UserId == id);

            if (user == null)
                return NotFound();

            // Optional: delete related expenses
            if (user.Expenses?.Any() == true)
                _context.Expenses.RemoveRange(user.Expenses);

            _context.User.Remove(user);
            _context.SaveChanges();
            TempData["Success"] = "🗑️ User deleted successfully!";

            return RedirectToAction(nameof(Index));
        }

        // View a single user's expenses
        public IActionResult ViewExpenses(int id)
        {
            var user = _context.User
                .Include(u => u.Expenses)
                .FirstOrDefault(u => u.UserId == id);

            if (user == null)
                return NotFound();

            ViewBag.UserName = user.Name;
            ViewBag.TotalExpenses = user.Expenses?.Sum(e => e.Value) ?? 0;

            return View(user.Expenses?.ToList() ?? new List<Expense>());
        }

       


    }

}

