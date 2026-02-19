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
using System.Collections.Generic;

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

        public IActionResult Cart()
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
            museum.TicketTypes ??= new List<TicketTypeModel>();

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

            // Clean up ticket types submitted from the form: remove empty rows
            if (museum.TicketTypes != null)
            {
                var cleaned = museum.TicketTypes
                    .Where(tt => !string.IsNullOrWhiteSpace(tt.Name) && tt.Price > 0)
                    .Select(tt => new TicketTypeModel
                    {
                        Name = tt.Name,
                        Price = tt.Price,
                        IsActive = tt.IsActive
                    })
                    .ToList();

                museum.TicketTypes = cleaned;
            }

            _context.Museums.Add(museum);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Catalogue(
            string searchString,
            string cityFilter,
            string typeFilter,
            int page = 1,
            int pageSize = 6)
        {
            if (pageSize <= 0) pageSize = 6;

            var query = _context.Museums
                .Include(m => m.Images)
                .AsNoTracking()
                .AsQueryable();

            // 🔎 GLOBAL SEARCH
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                query = query.Where(m =>
                    m.Name.Contains(searchString) ||
                    m.City.Contains(searchString) ||
                    m.Type.Contains(searchString) ||
                    m.Description.Contains(searchString) ||
                    m.Address.Contains(searchString));
            }

            // 🏙 CITY FILTER
            if (!string.IsNullOrWhiteSpace(cityFilter))
            {
                query = query.Where(m => m.City == cityFilter);
            }

            // 🏛 TYPE FILTER
            if (!string.IsNullOrWhiteSpace(typeFilter))
            {
                query = query.Where(m => m.Type == typeFilter);
            }

            query = query.OrderBy(m => m.MuseumId);

            var totalCount = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            if (totalPages == 0) totalPages = 1;

            page = Math.Clamp(page, 1, totalPages);

            var museums = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Send filter values back to View
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.SearchString = searchString;
            ViewBag.CityFilter = cityFilter;
            ViewBag.TypeFilter = typeFilter;

            // For dropdown lists
            ViewBag.Cities = await _context.Museums
                .Select(m => m.City)
                .Distinct()
                .ToListAsync();

            ViewBag.Types = await _context.Museums
                .Select(m => m.Type)
                .Distinct()
                .ToListAsync();

            return View(museums);
        }


        public async Task<IActionResult> Dashboard(
            string searchString,
            string cityFilter,
            string typeFilter,
            int page = 1,
            int pageSize = 6)
        {
            if (pageSize <= 0) pageSize = 6;

            var query = _context.Museums
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                query = query.Where(m =>
                    m.Name.Contains(searchString) ||
                    m.City.Contains(searchString) ||
                    m.Type.Contains(searchString) ||
                    m.Description.Contains(searchString) ||
                    m.Address.Contains(searchString));
            }

            if (!string.IsNullOrWhiteSpace(cityFilter))
            {
                query = query.Where(m => m.City == cityFilter);
            }

            if (!string.IsNullOrWhiteSpace(typeFilter))
            {
                query = query.Where(m => m.Type == typeFilter);
            }

            query = query.OrderBy(m => m.MuseumId);

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
            ViewBag.SearchString = searchString;
            ViewBag.CityFilter = cityFilter;
            ViewBag.TypeFilter = typeFilter;

            ViewBag.Cities = await _context.Museums
                .Select(m => m.City)
                .Distinct()
                .ToListAsync();

            ViewBag.Types = await _context.Museums
                .Select(m => m.Type)
                .Distinct()
                .ToListAsync();

            return View(museums);
        }

        public async Task<IActionResult> Details(int id)
        {
            var museum = await _context.Museums
                .Include(m => m.Images)
                .Include(m => m.TicketTypes)
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

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BuyTickets([FromForm] OrderCreateDto dto)
        {
            if (dto == null)
            {
                return BadRequest();
            }

            // Basic validation
            if (dto.Items == null || !dto.Items.Any(i => i.Quantity > 0))
            {
                TempData["Error"] = "Select at least one ticket.";
                return RedirectToAction(nameof(Details), new { id = dto.MuseumId });
            }

            var museum = await _context.Museums.FindAsync(dto.MuseumId);
            if (museum == null)
            {
                return NotFound();
            }

            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out var userId))
            {
                return Challenge(); // not authenticated or invalid id
            }

            var order = new OrderModel
            {
                OrderCode = Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper(),
                VisitDate = dto.VisitDate.Date,
                CreatedAt = DateTime.Now,
                MuseumId = dto.MuseumId,
                UserId = userId,
                OrderItems = new List<OrderItemModel>()
            };

            foreach (var item in dto.Items.Where(i => i.Quantity > 0))
            {
                var tt = await _context.TicketTypes.FindAsync(item.TicketTypeId);
                if (tt == null)
                {
                    // skip invalid ticket types
                    continue;
                }

                order.OrderItems.Add(new OrderItemModel
                {
                    TicketTypeId = item.TicketTypeId,
                    Quantity = item.Quantity,
                    PriceAtPurchase = tt.Price
                });
            }

            if (!order.OrderItems.Any())
            {
                TempData["Error"] = "No valid tickets selected.";
                return RedirectToAction(nameof(Details), new { id = dto.MuseumId });
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            TempData["Success"] = $"Order {order.OrderCode} created. Visit date: {order.VisitDate:yyyy-MM-dd}.";
            return RedirectToAction(nameof(Details), new { id = dto.MuseumId });
        }
    }
}
