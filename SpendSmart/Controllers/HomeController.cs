using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SpendSmart.Models;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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
            return View();
        }

        public IActionResult Listed(string sortOrder)
        {
            var expenses = _context.Expenses
                .AsNoTracking() // Avoid unnecessary change tracking
                .Select(expense => new Expense
                {
                    Id = expense.Id,
                    Value = expense.Value,
                    Description = expense.Description ?? string.Empty,
                    ImagePath = expense.ImagePath ?? string.Empty // Handle null image path
                });

            // Sort order
            switch (sortOrder)
            {
                case "value_asc":
                    expenses = expenses.OrderBy(e => e.Value);
                    break;
                case "value_desc":
                    expenses = expenses.OrderByDescending(e => e.Value);
                    break;
                default:
                    expenses = expenses.OrderByDescending(e => e.Id);
                    break;
            }

            return View(expenses.ToList());
        }


        public IActionResult Expenses()
        {
            var allExpenses = _context.Expenses
                .AsNoTracking() // Avoid unnecessary change tracking
                .Select(expense => new Expense
                {
                    Id = expense.Id,
                    Value = expense.Value,
                    Description = expense.Description ?? string.Empty, // Handle null description
                    ImagePath = expense.ImagePath ?? string.Empty // Handle null image path
                })
                .ToList();

            return View(allExpenses);
        }

        public IActionResult CreateEditExpense(int? id)
        {
            if (id.HasValue)
            {
                var expenseInDb = _context.Expenses
                    .AsNoTracking()
                    .FirstOrDefault(x => x.Id == id);

                if (expenseInDb != null)
                {
                    return View(expenseInDb);
                }

                return NotFound(); // Eğer expense bulunamazsa 404 döndür
            }

            return View(new Expense()); // Yeni bir Expense nesnesi oluştur
        }


        [HttpPost]
        public async Task<IActionResult> CreateEditExpenseForm(Expense model)
        {
            if (ModelState.IsValid)
            {
                if (model.UploadedImage != null)
                {
                    var fileName = Path.GetFileName(model.UploadedImage.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.UploadedImage.CopyToAsync(stream);
                    }

                    // Save the file path to the database
                    model.ImagePath = "/images/" + fileName;
                }

                if (model.Id == 0)
                {
                    _context.Expenses.Add(model);
                }
                else
                {
                    _context.Expenses.Update(model);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction("Expenses"); // Redirect to Expenses after saving
            }

            // If we got this far, something failed, redisplay form
            return View("CreateEditExpense", model);
        }


        public IActionResult DeleteExpense(int id)
        {
            // Veritabanından id ile eşleşen kaydı getiriyoruz
            var expenseInDb = _context.Expenses.SingleOrDefault(x => x.Id == id);

            if (expenseInDb == null)
            {
                return NotFound();
            }

            // ImagePath'in null veya boş olup olmadığını kontrol ediyoruz
            if (!string.IsNullOrEmpty(expenseInDb.ImagePath))
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", expenseInDb.ImagePath.TrimStart('/'));
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            // Veritabanındaki kaydı siliyoruz
            _context.Expenses.Remove(expenseInDb);
            _context.SaveChanges();

            return RedirectToAction("Expenses");
        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
