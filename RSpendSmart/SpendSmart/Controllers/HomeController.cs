using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SpendSmart.Models;

namespace SpendSmart.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SpendSmartDbContext _context;

        public HomeController(ILogger<HomeController> logger, SpendSmartDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var expenses = _context.Expenses
                .Include(e => e.User)
                .ToList();

            return View(expenses);
        }

        public IActionResult Expenses()
        {
            var allExpenses = _context.Expenses
                .Include(e => e.User)
                .ToList();

            var totalExpenses = allExpenses.Sum(x => x.Value);
            ViewBag.TotalExpenses = totalExpenses;

            return View(allExpenses ?? new List<Expense>());
        }

        // GET: Create/Edit Expense Form
        public IActionResult CreateEditExpenseForm(int? id)
        {
            ViewBag.Users = new SelectList(_context.User, "UserId", "Name");

            // If no ID ? Create new expense
            if (id == null)
            {
                return View("CreateEditExpense", new Expense());
            }

            // If ID exists ? Edit existing expense
            var expenseInDb = _context.Expenses.SingleOrDefault(e => e.Id == id);
            if (expenseInDb == null)
            {
                return NotFound();
            }

            return View("CreateEditExpense", expenseInDb);
        }

        // POST: Create or Edit Expense
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateEditExpense(Expense model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.Users = new SelectList(_context.User, "UserId", "Name", model.UserId);
                    TempData["Error"] = "Please fix the errors before submitting.";
                    return View("CreateEditExpense", model);
                }

                if (model.UserId == 0)
                {
                    ModelState.AddModelError("UserId", "Please select a user.");
                    ViewBag.Users = new SelectList(_context.User, "UserId", "Name", model.UserId);
                    TempData["Error"] = "User selection is required.";
                    return View("CreateEditExpense", model);
                }

                if (model.Id == 0)
                {
                    _context.Expenses.Add(model);
                    TempData["Success"] = "Expense created successfully!";
                }
                else
                {
                    _context.Expenses.Update(model);
                    TempData["Success"] = "Expense updated successfully!";
                }

                _context.SaveChanges();
                return RedirectToAction("Expenses");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Something went wrong while saving the expense.";
                Console.WriteLine(ex.Message);
                return RedirectToAction("Expenses");
            }
        }

        // DELETE
        public IActionResult DeleteExpense(int id)
        {
            var expenseInDb = _context.Expenses.SingleOrDefault(e => e.Id == id);

            if (expenseInDb == null)
            {
                TempData["Error"] = "Expense not found.";
                return RedirectToAction("Expenses");
            }

            _context.Expenses.Remove(expenseInDb);
            _context.SaveChanges();
            TempData["Success"] = "Expense deleted successfully!";
            return RedirectToAction("Expenses");
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}
