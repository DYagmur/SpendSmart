using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpendSmart.Models;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SpendSmart.Controllers
{
    public class HomeController : Controller
    {
        private readonly SqlServerDbContext _sqlServerDbContext;
        private readonly PostgreSqlDbContext _postgreSqlDbContext;

        public HomeController(SqlServerDbContext sqlServerDbContext, PostgreSqlDbContext postgreSqlDbContext)
        {
            _sqlServerDbContext = sqlServerDbContext;
            _postgreSqlDbContext = postgreSqlDbContext;
        }

        public async Task<IActionResult> Index()
        {
            var expenses = await _sqlServerDbContext.Expenses.ToListAsync();
            return View(expenses);
        }

        public IActionResult Listed(string sortOrder)
        {
            var expenses = _sqlServerDbContext.Expenses
                .AsNoTracking()
                .Select(expense => new Expense
                {
                    Id = expense.Id,
                    Value = expense.Value,
                    Description = expense.Description ?? string.Empty,
                    SmallImagePath = expense.SmallImagePath ?? string.Empty,
                    MediumImagePath = expense.MediumImagePath ?? string.Empty,
                    LargeImagePath = expense.LargeImagePath ?? string.Empty
                });

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
            var allExpenses = _sqlServerDbContext.Expenses
                .AsNoTracking()
                .Select(expense => new Expense
                {
                    Id = expense.Id,
                    Value = expense.Value,
                    Description = expense.Description ?? string.Empty,
                    SmallImagePath = expense.SmallImagePath ?? string.Empty,
                    MediumImagePath = expense.MediumImagePath ?? string.Empty,
                    LargeImagePath = expense.LargeImagePath ?? string.Empty
                })
                .ToList();

            return View(allExpenses);
        }

        public IActionResult CreateEditExpense(int? id)
        {
            if (id.HasValue)
            {
                var expenseInDb = _sqlServerDbContext.Expenses
                    .AsNoTracking()
                    .FirstOrDefault(x => x.Id == id);

                if (expenseInDb != null)
                {
                    return View(expenseInDb);
                }

                return NotFound();
            }

            return View(new Expense());
        }

        [HttpPost]
        public async Task<IActionResult> CreateEditExpenseForm(Expense model)
        {
            if (ModelState.IsValid)
            {
                if (model.UploadedImage != null)
                {
                    var fileName = Path.GetFileNameWithoutExtension(model.UploadedImage.FileName);
                    var extension = Path.GetExtension(model.UploadedImage.FileName);

                    // Define paths for small, medium, and large images
                    var smallImagePath = Path.Combine("wwwroot/images", fileName + "-small" + extension);
                    var mediumImagePath = Path.Combine("wwwroot/images", fileName + "-medium" + extension);
                    var largeImagePath = Path.Combine("wwwroot/images", fileName + "-large" + extension);

                    // Process and save the different image sizes
                    using (var stream = new FileStream(Path.Combine(Directory.GetCurrentDirectory(), smallImagePath), FileMode.Create))
                    {
                        var smallImage = ResizeImage(model.UploadedImage.OpenReadStream(), 100, 100);
                        smallImage.Save(stream, ImageFormat.Jpeg);
                    }

                    using (var stream = new FileStream(Path.Combine(Directory.GetCurrentDirectory(), mediumImagePath), FileMode.Create))
                    {
                        var mediumImage = ResizeImage(model.UploadedImage.OpenReadStream(), 500, 500);
                        mediumImage.Save(stream, ImageFormat.Jpeg);
                    }

                    using (var stream = new FileStream(Path.Combine(Directory.GetCurrentDirectory(), largeImagePath), FileMode.Create))
                    {
                        var largeImage = ResizeImage(model.UploadedImage.OpenReadStream(), 1000, 1000);
                        largeImage.Save(stream, ImageFormat.Jpeg);
                    }

                    // Save the file paths to the database
                    model.SmallImagePath = "/images/" + Path.GetFileName(smallImagePath);
                    model.MediumImagePath = "/images/" + Path.GetFileName(mediumImagePath);
                    model.LargeImagePath = "/images/" + Path.GetFileName(largeImagePath);
                }

                if (model.Id == 0)
                {
                    _sqlServerDbContext.Expenses.Add(model);
                }
                else
                {
                    _sqlServerDbContext.Expenses.Update(model);
                }

                await _sqlServerDbContext.SaveChangesAsync();
                return RedirectToAction("Expenses");
            }

            return View("CreateEditExpense", model);
        }

        private Image ResizeImage(Stream stream, int width, int height)
        {
            var image = Image.FromStream(stream);
            var resized = new Bitmap(width, height);

            using (var graphics = Graphics.FromImage(resized))
            {
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.DrawImage(image, 0, 0, width, height);
            }

            return resized;
        }

        public IActionResult DeleteExpense(int id)
        {
            var expenseInDb = _sqlServerDbContext.Expenses.SingleOrDefault(x => x.Id == id);

            if (expenseInDb == null)
            {
                return NotFound();
            }

            // Delete image files if they exist
            var imagePaths = new[] { expenseInDb.SmallImagePath, expenseInDb.MediumImagePath, expenseInDb.LargeImagePath };
            foreach (var imagePath in imagePaths)
            {
                if (!string.IsNullOrEmpty(imagePath))
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imagePath.TrimStart('/'));
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }
            }

            _sqlServerDbContext.Expenses.Remove(expenseInDb);
            _sqlServerDbContext.SaveChanges();

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
