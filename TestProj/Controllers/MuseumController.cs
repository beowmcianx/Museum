using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TestProj.Data;
using TestProj.Models;
using TestProj.Models.NewFolder;
using TestProj.Models.ViewModels;

namespace TestProj.Controllers
{
    public class MuseumController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserModel> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;

        public MuseumController(
            ApplicationDbContext context,
            UserManager<UserModel> userManager,
            RoleManager<IdentityRole<int>> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Catalogue(string? searchString, string? countryFilter, string? cityFilter, string? typeFilter, int page = 1, int pageSize = 6)
        {
            // Preserve current filter/search values for the view
            ViewBag.SearchString = searchString ?? string.Empty;
            ViewBag.CountryFilter = countryFilter ?? string.Empty;
            ViewBag.CityFilter = cityFilter ?? string.Empty;
            ViewBag.TypeFilter = typeFilter ?? string.Empty;

            // Provide lists for the filter dropdowns (distinct non-empty values)
            // Cities (DEPENDENT on selected country)
            if (!string.IsNullOrWhiteSpace(countryFilter))
            {
                ViewBag.Cities = await _context.Museums
                    .AsNoTracking()
                    .Where(m => m.Country == countryFilter && !string.IsNullOrEmpty(m.City))
                    .Select(m => m.City!)
                    .Distinct()
                    .OrderBy(c => c)
                    .ToListAsync();
            }
            else
            {
                ViewBag.Cities = await _context.Museums
                    .AsNoTracking()
                    .Where(m => !string.IsNullOrEmpty(m.City))
                    .Select(m => m.City!)
                    .Distinct()
                    .OrderBy(c => c)
                    .ToListAsync();
            }

            ViewBag.Countries = await _context.Museums
               .AsNoTracking()
               .Where(m => !string.IsNullOrEmpty(m.Country))
               .Select(m => m.Country!)
               .Distinct()
               .OrderBy(c => c)
               .ToListAsync();

            ViewBag.Types = await _context.Museums
                .AsNoTracking()
                .Where(m => !string.IsNullOrEmpty(m.Type))
                .Select(m => m.Type!)
                .Distinct()
                .OrderBy(t => t)
                .ToListAsync();

            // Base query (include images)
            IQueryable<MuseumModel> query = _context.Museums
                .Include(m => m.Images)
                .AsNoTracking();

            // Apply search
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                var s = $"%{searchString.Trim()}%";
                query = query.Where(m =>
                    EF.Functions.Like(m.Name, s) ||
                    EF.Functions.Like(m.Description, s) ||
                    EF.Functions.Like(m.Country, s) ||
                    EF.Functions.Like(m.City, s) ||
                    EF.Functions.Like(m.Type, s));
            }

            // Apply city/type filters
            if (!string.IsNullOrWhiteSpace(countryFilter))
            {
                query = query.Where(m => m.Country == countryFilter);
            }

            if (!string.IsNullOrWhiteSpace(cityFilter))
            {
                query = query.Where(m => m.City == cityFilter);
            }

            if (!string.IsNullOrWhiteSpace(typeFilter))
            {
                query = query.Where(m => m.Type == typeFilter);
            }

            // Ordering + pagination
            query = query.OrderBy(m => m.MuseumId);

            var totalCount = await query.CountAsync();
            var totalPages = Math.Max(1, (int)Math.Ceiling(totalCount / (double)pageSize));
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

        public async Task<IActionResult> Details(int id)
        {
            // Include TicketTypes so the Details view can display available ticket types
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
        [Authorize(Roles = Roles.Client)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BuyTickets(OrderCreateDto input)
        {
            if (!input.AcceptRules)
            {
                TempData["Error"] = "You must accept the rules before purchasing.";
                return RedirectToAction(nameof(Details), new { id = input.MuseumId });
            }

            if (input == null)
            {
                TempData["Error"] = "Invalid request.";
                return RedirectToAction(nameof(Catalogue));
            }

            // get current user
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                TempData["Error"] = "You must be signed in to buy tickets.";
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }

            // Validate museum exists
            var museum = await _context.Museums
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.MuseumId == input.MuseumId);

            if (museum == null)
            {
                TempData["Error"] = "Museum not found.";
                return RedirectToAction(nameof(Catalogue));
            }

            // Filter selected items (quantity > 0)
            var selected = input.Items?.Where(i => i.Quantity > 0).ToList() ?? new List<OrderItemInput>();
            if (!selected.Any())
            {
                TempData["Error"] = "No tickets selected.";
                return RedirectToAction(nameof(Details), new { id = input.MuseumId });
            }

            // Load ticket types used for the order
            var ticketTypeIds = selected.Select(i => i.TicketTypeId).Distinct().ToList();
            var ticketTypes = await _context.TicketTypes
                .Where(tt => ticketTypeIds.Contains(tt.TicketTypeId))
                .ToListAsync();

            // Ensure all requested ticket types are valid and belong to this museum
            foreach (var sel in selected)
            {
                var tt = ticketTypes.FirstOrDefault(t => t.TicketTypeId == sel.TicketTypeId);
                if (tt == null || tt.MuseumId != input.MuseumId)
                {
                    TempData["Error"] = "One or more selected ticket types are invalid.";
                    return RedirectToAction(nameof(Details), new { id = input.MuseumId });
                }
            }

            // Create order and order items
            var order = new OrderModel
            {
                OrderCode = $"ORD-{Guid.NewGuid().ToString("N").Substring(0, 8).ToUpperInvariant()}",
                VisitDate = input.VisitDate.Date,
                UserId = user.Id,
                MuseumId = input.MuseumId,
                Status = OrderStatus.New,
                CreatedAt = DateTime.Now,
                OrderItems = new List<OrderItemModel>()
            };

            foreach (var sel in selected)
            {
                var tt = ticketTypes.First(t => t.TicketTypeId == sel.TicketTypeId);
                var oi = new OrderItemModel
                {
                    TicketTypeId = tt.TicketTypeId,
                    Quantity = sel.Quantity,
                    PriceAtPurchase = tt.Price
                };
                order.OrderItems.Add(oi);
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Purchase successful. Order code: " + order.OrderCode;
            return RedirectToAction(nameof(Details), new { id = input.MuseumId });
        }
    }
}
