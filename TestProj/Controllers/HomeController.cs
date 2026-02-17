using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TestProj.Data;
using TestProj.Models;

namespace TestProj.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public HomeController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Create()
        {
            ViewBag.Museums = await _context.Museums.ToListAsync();

            return View(new MuseumModel());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> Index(MuseumModel museum)
        {

            if (!ModelState.IsValid)
            {
                ViewBag.Museums = await _context.Museums.ToListAsync();
                return View("Index", museum);
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MuseumModel museum, IFormFile? imageFile)
        {
            ModelState.Remove(nameof(MuseumModel.TicketTypes));
            ModelState.Remove(nameof(MuseumModel.Orders));
            ModelState.Remove(nameof(MuseumModel.Employees));
            ModelState.Remove(nameof(MuseumModel.Images));

            if (!ModelState.IsValid)
            {
                ViewBag.Museums = await _context.Museums.ToListAsync();
                return View("Index", museum);
            }

            museum.Images ??= new System.Collections.Generic.List<MuseumImageModel>();

            if (imageFile != null && imageFile.Length > 0)
            {
                // validate content-type and size as needed (example below)
                var permittedTypes = new[] { "image/jpeg", "image/png", "image/gif" };
                if (!permittedTypes.Contains(imageFile.ContentType))
                {
                    ModelState.AddModelError("imageFile", "Only JPG/PNG/GIF images are allowed.");
                    ViewBag.Museums = await _context.Museums.ToListAsync();
                    return View(museum);
                }

                var imagesFolder = Path.Combine(_env.WebRootPath, "images", "museums");
                if (!Directory.Exists(imagesFolder))
                {
                    Directory.CreateDirectory(imagesFolder);
                }

                var ext = Path.GetExtension(imageFile.FileName);
                var fileName = $"{Guid.NewGuid()}{ext}";
                var fullPath = Path.Combine(imagesFolder, fileName);

                await using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                var imageUrl = $"/images/museums/{fileName}";
                museum.Images.Add(new MuseumImageModel
                {
                    ImageUrl = imageUrl
                });
            }

            _context.Museums.Add(museum);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // Catalogue with pagination and images included
        // URL example: /Home/Catalogue?page=1
        public async Task<IActionResult> Catalogue(int page = 1, int pageSize = 6)
        {
            if (pageSize <= 0) pageSize = 6;

            var query = _context.Museums
                .Include(m => m.Images)
                .AsNoTracking()
                .OrderBy(m => m.MuseumId);

            var totalCount = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            if (totalPages == 0) totalPages = 1;

            page = Math.Clamp(page, 1, totalPages);

            var museums = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.PageSize = pageSize;

            return View(museums);
        }

        public async Task<IActionResult> Dashboard()
        {
            ViewBag.Museums = await _context.Museums.ToListAsync();
            return View();
        }

        public async Task<IActionResult> Details(int id)
        {
            var museum = await _context.Museums
                .Include(m => m.Images)
                .FirstOrDefaultAsync(m => m.MuseumId == id);

            if (museum == null)
            {
                return NotFound();
            }

            return View(museum);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var museum = await _context.Museums
                .FirstOrDefaultAsync(m => m.MuseumId == id);

            if (museum == null)
            {
                return NotFound();
            }

            _context.Museums.Remove(museum);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var museum = await _context.Museums
                .FirstOrDefaultAsync(m => m.MuseumId == id);

            if (museum == null)
            {
                return NotFound();
            }

            return View(museum);
        }

        // Diagnostic endpoint. Visit /Home/WhoAmI to see current identity and roles.
        public IActionResult WhoAmI()
        {
            var name = User.Identity?.Name ?? "(not authenticated)";
            var isAuthenticated = User.Identity?.IsAuthenticated ?? false;
            var roleClaims = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
            var rolesText = roleClaims.Any() ? string.Join(", ", roleClaims) : "(no role claims)";
            var claimsText = string.Join(" | ", User.Claims.Select(c => $"{c.Type}={c.Value}"));

            var text = $"Name: {name}\nAuthenticated: {isAuthenticated}\nRoles: {rolesText}\nClaims: {claimsText}";
            return Content(text, "text/plain");
        }
    }
}
