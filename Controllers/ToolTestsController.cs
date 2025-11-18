using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToolCaptureAppClean.Models;

namespace ToolCaptureAppClean.Controllers
{
    public class ToolTestsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ToolTestsController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // ✅ INDEX
        public async Task<IActionResult> Index()
        {
            var data = await _context.ToolTests
                .Include(t => t.BrandResults)
                .OrderByDescending(t => t.Id)
                .ToListAsync();
            return View(data);
        }

        // ✅ DETAILS
        public async Task<IActionResult> Details(int id)
        {
            var model = await _context.ToolTests
                .Include(t => t.BrandResults)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (model == null) return NotFound();
            return View(model);
        }

        // ✅ CREATE (GET)
        public IActionResult Create() => View();

        // ✅ CREATE (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            ToolTest toolTest,
            List<IFormFile>? photos,
            string? capturedImages,
            List<BrandResult>? Brands)
        {
            if (!ModelState.IsValid) return View(toolTest);

            // Ensure upload folder exists
            var uploadRoot = Path.Combine(_env.WebRootPath, "uploads");
            Directory.CreateDirectory(uploadRoot);

            // --- Handle normal file uploads ---
            if (photos != null && photos.Count > 0)
            {
                var paths = new List<string>();
                foreach (var photo in photos)
                {
                    var fileName = $"{DateTime.UtcNow:yyyyMMdd_HHmmssfff}_{Path.GetFileName(photo.FileName)}";
                    var filePath = Path.Combine(uploadRoot, fileName);
                    using var stream = System.IO.File.Create(filePath);
                    await photo.CopyToAsync(stream);
                    paths.Add($"/uploads/{fileName}");
                }
                toolTest.PhotoPath = string.Join(";", paths);
            }
            // --- Handle webcam base64 images ---
            else if (!string.IsNullOrWhiteSpace(capturedImages))
            {
                var list = System.Text.Json.JsonSerializer.Deserialize<List<string>>(capturedImages);
                var paths = new List<string>();
                foreach (var b64 in list ?? new())
                {
                    var fileName = $"{DateTime.UtcNow:yyyyMMdd_HHmmssfff}_webcam.png";
                    var filePath = Path.Combine(uploadRoot, fileName);
                    var bytes = Convert.FromBase64String(b64.Split(',')[1]);
                    await System.IO.File.WriteAllBytesAsync(filePath, bytes);
                    paths.Add($"/uploads/{fileName}");
                }
                toolTest.PhotoPath = string.Join(";", paths);
            }

            _context.Add(toolTest);
            await _context.SaveChangesAsync();

            // --- Handle brand results ---
            if (Brands != null && Brands.Count > 0)
            {
                foreach (var br in Brands)
                {
                    br.ToolTestId = toolTest.Id;
                    _context.BrandResults.Add(br);
                }
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // ✅ EDIT (GET)
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var toolTest = await _context.ToolTests.FindAsync(id);
            if (toolTest == null)
                return NotFound();

            return View(toolTest);
        }
        // ✅ EDIT (POST with photo + webcam support)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            ToolTest toolTest,
            List<IFormFile>? photos,
            string? capturedImages)
        {
            if (id != toolTest.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(toolTest);

            try
            {
                var dbItem = await _context.ToolTests.FirstOrDefaultAsync(t => t.Id == id);
                if (dbItem == null)
                    return NotFound();

                // --- Update fields ---
                dbItem.ToolId = toolTest.ToolId;
                dbItem.TestDate = toolTest.TestDate;
                dbItem.Operator = toolTest.Operator;
                dbItem.Customer = toolTest.Customer;
                dbItem.Material = toolTest.Material;
                dbItem.Operation = toolTest.Operation;
                dbItem.Coolant = toolTest.Coolant;
                dbItem.Notes = toolTest.Notes;

                var uploadRoot = Path.Combine(_env.WebRootPath, "uploads");
                Directory.CreateDirectory(uploadRoot);
                var newPhotos = new List<string>();

                // --- Case 1: Upload new files ---
                if (photos != null && photos.Count > 0)
                {
                    foreach (var photo in photos)
                    {
                        var fileName = $"{DateTime.UtcNow:yyyyMMdd_HHmmssfff}_{Path.GetFileName(photo.FileName)}";
                        var filePath = Path.Combine(uploadRoot, fileName);
                        using var stream = System.IO.File.Create(filePath);
                        await photo.CopyToAsync(stream);
                        newPhotos.Add($"/uploads/{fileName}");
                    }
                }

                // --- Case 2: Webcam (base64) ---
                if (!string.IsNullOrWhiteSpace(capturedImages))
                {
                    try
                    {
                        var list = System.Text.Json.JsonSerializer.Deserialize<List<string>>(capturedImages);
                        foreach (var base64 in list ?? new())
                        {
                            if (base64.StartsWith("data:image"))
                            {
                                var fileName = $"{DateTime.UtcNow:yyyyMMdd_HHmmssfff}_webcam.png";
                                var filePath = Path.Combine(uploadRoot, fileName);
                                var bytes = Convert.FromBase64String(base64.Split(',')[1]);
                                await System.IO.File.WriteAllBytesAsync(filePath, bytes);
                                newPhotos.Add($"/uploads/{fileName}");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("❌ Webcam decode error: " + ex.Message);
                    }
                }

                // --- Only replace if new photos exist ---
                if (newPhotos.Any())
                {
                    dbItem.PhotoPath = string.Join(";", newPhotos);
                }

                _context.Update(dbItem);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.ToolTests.Any(e => e.Id == toolTest.Id))
                    return NotFound();
                else
                    throw;
            }
        }

        // ✅ DELETE (GET)
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var toolTest = await _context.ToolTests
                .FirstOrDefaultAsync(m => m.Id == id);

            if (toolTest == null)
                return NotFound();

            return View(toolTest);
        }

        // ✅ DELETE (POST)
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var toolTest = await _context.ToolTests.FindAsync(id);
            if (toolTest != null)
            {
                _context.ToolTests.Remove(toolTest);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
