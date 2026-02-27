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
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserModel> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;

        public HomeController(
            ApplicationDbContext context,
            UserManager<UserModel> userManager,
            RoleManager<IdentityRole<int>> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
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
        [Authorize(Roles = Roles.Admin)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MuseumModel museum)
        {
            ModelState.Remove(nameof(MuseumModel.TicketTypes));
            ModelState.Remove(nameof(MuseumModel.Orders));
            ModelState.Remove(nameof(MuseumModel.Employees));
            ModelState.Remove(nameof(MuseumModel.Images));

            // Remove nested collection ModelState entries that commonly block saving
            foreach (var key in ModelState.Keys.Where(k =>
                         k.StartsWith("TicketTypes[", StringComparison.OrdinalIgnoreCase) ||
                         k.StartsWith("Images[", StringComparison.OrdinalIgnoreCase) ||
                         k.StartsWith("Orders[", StringComparison.OrdinalIgnoreCase) ||
                         k.StartsWith("Employees[", StringComparison.OrdinalIgnoreCase))
                     .ToList())
            {
                ModelState.Remove(key);
            }

            // Robustly parse OpeningTime / ClosingTime as before...
            if (Request.Form.TryGetValue("OpeningTime", out var openingVal))
            {
                var s = openingVal.ToString();
                if (!string.IsNullOrWhiteSpace(s))
                {
                    if (DateTime.TryParse(s, out var dt))
                    {
                        museum.OpeningTime = dt.TimeOfDay;
                        ModelState.Remove(nameof(MuseumModel.OpeningTime));
                    }
                    else if (TimeSpan.TryParse(s, out var ts))
                    {
                        museum.OpeningTime = ts;
                        ModelState.Remove(nameof(MuseumModel.OpeningTime));
                    }
                    else
                    {
                        ModelState.Remove(nameof(MuseumModel.OpeningTime));
                        museum.OpeningTime = TimeSpan.Zero;
                    }
                }
                else
                {
                    ModelState.Remove(nameof(MuseumModel.OpeningTime));
                    museum.OpeningTime = TimeSpan.Zero;
                }
            }
            else
            {
                ModelState.Remove(nameof(MuseumModel.OpeningTime));
                museum.OpeningTime = TimeSpan.Zero;
            }

            if (Request.Form.TryGetValue("ClosingTime", out var closingVal))
            {
                var s = closingVal.ToString();
                if (!string.IsNullOrWhiteSpace(s))
                {
                    if (DateTime.TryParse(s, out var dt))
                    {
                        museum.ClosingTime = dt.TimeOfDay;
                        ModelState.Remove(nameof(MuseumModel.ClosingTime));
                    }
                    else if (TimeSpan.TryParse(s, out var ts))
                    {
                        museum.ClosingTime = ts;
                        ModelState.Remove(nameof(MuseumModel.ClosingTime));
                    }
                    else
                    {
                        ModelState.Remove(nameof(MuseumModel.ClosingTime));
                        museum.ClosingTime = TimeSpan.Zero;
                    }
                }
                else
                {
                    ModelState.Remove(nameof(MuseumModel.ClosingTime));
                    museum.ClosingTime = TimeSpan.Zero;
                }
            }
            else
            {
                ModelState.Remove(nameof(MuseumModel.ClosingTime));
                museum.ClosingTime = TimeSpan.Zero;
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Museums = await _context.Museums.ToListAsync();
                return View("Create", museum);
            }

            // If an image URL was provided, attach it to the new museum as a MuseumImageModel.
            // If no image provided, use default "/images/imagenotfound.png".
            var imageUrl = Request.Form["ImageUrl"].ToString();
            museum.Images = museum.Images ?? new List<MuseumImageModel>();
            if (!string.IsNullOrWhiteSpace(imageUrl))
            {
                museum.Images.Add(new MuseumImageModel
                {
                    ImageUrl = imageUrl
                });
            }
            else
            {
                // default image when user did not provide one
                museum.Images.Add(new MuseumImageModel
                {
                    ImageUrl = "/images/imagenotfound.png"
                });
            }

            // Parse posted TicketTypes from the form and attach to the museum so they are saved
            var ttKeys = Request.Form.Keys
                .Where(k => k.StartsWith("TicketTypes[", StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (ttKeys.Any())
            {
                museum.TicketTypes = museum.TicketTypes ?? new List<TicketTypeModel>();

                // extract indices like TicketTypes[0].Name -> 0
                var indices = ttKeys
                    .Select(k =>
                    {
                        var start = k.IndexOf('[');
                        var end = k.IndexOf(']');
                        if (start >= 0 && end > start) return k.Substring(start + 1, end - start - 1);
                        return null;
                    })
                    .Where(s => !string.IsNullOrEmpty(s))
                    .Distinct();

                foreach (var idx in indices)
                {
                    var name = Request.Form[$"TicketTypes[{idx}].Name"].ToString();
                    var priceStr = Request.Form[$"TicketTypes[{idx}].Price"].ToString();
                    var isActivePresent = Request.Form.ContainsKey($"TicketTypes[{idx}].IsActive");

                    if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(priceStr))
                        continue;

                    if (!decimal.TryParse(priceStr, NumberStyles.Number | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var price))
                    {
                        // try current culture as fallback
                        if (!decimal.TryParse(priceStr, NumberStyles.Number | NumberStyles.AllowDecimalPoint, CultureInfo.CurrentCulture, out price))
                        {
                            continue;
                        }
                    }

                    var tt = new TicketTypeModel
                    {
                        Name = name.Trim(),
                        Price = price,
                        IsActive = isActivePresent,
                        Museum = museum // ensure the relationship is established for EF
                    };

                    museum.TicketTypes.Add(tt);
                }
            }

            // Add the museum and related child entities (ticket types if you later add them)
            _context.Museums.Add(museum);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Catalogue(string? searchString, string? countryFilter, string? cityFilter, string? typeFilter, int page = 1, int pageSize = 6)
        {
            // Preserve current filter/search values for the view
            ViewBag.SearchString = searchString ?? string.Empty;
            ViewBag.CountryFilter = countryFilter ?? string.Empty;
            ViewBag.CityFilter = cityFilter ?? string.Empty;
            ViewBag.TypeFilter = typeFilter ?? string.Empty;

            // Provide lists for the filter dropdowns (distinct non-empty values)
            ViewBag.Cities = await _context.Museums
                .AsNoTracking()
                .Where(m => !string.IsNullOrEmpty(m.City))
                .Select(m => m.City!)
                .Distinct()
                .OrderBy(c => c)
                .ToListAsync();

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

        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Dashboard(
            string? searchString,
            string? countryFilter,
            string? cityFilter,
            string? typeFilter,
            int page = 1,
            int pageSize = 10)
        {
            // Preserve filter values
            ViewBag.SearchString = searchString ?? string.Empty;
            ViewBag.CountryFilter = countryFilter ?? string.Empty;
            ViewBag.CityFilter = cityFilter ?? string.Empty;
            ViewBag.TypeFilter = typeFilter ?? string.Empty;

            // Dropdown lists
            ViewBag.Countries = await _context.Museums
                .AsNoTracking()
                .Where(m => !string.IsNullOrEmpty(m.Country))
                .Select(m => m.Country!)
                .Distinct()
                .OrderBy(c => c)
                .ToListAsync();

            ViewBag.Cities = await _context.Museums
                .AsNoTracking()
                .Where(m => !string.IsNullOrEmpty(m.City))
                .Select(m => m.City!)
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

            IQueryable<MuseumModel> query = _context.Museums
                .Include(m => m.Images)
                .AsNoTracking();

            // Search
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

            // Filters
            if (!string.IsNullOrWhiteSpace(countryFilter))
                query = query.Where(m => m.Country == countryFilter);

            if (!string.IsNullOrWhiteSpace(cityFilter))
                query = query.Where(m => m.City == cityFilter);

            if (!string.IsNullOrWhiteSpace(typeFilter))
                query = query.Where(m => m.Type == typeFilter);

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

        public async Task<IActionResult> Museum(int id)
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

        [HttpGet]
        [Authorize(Roles = Roles.Client)]
        public async Task<IActionResult> Cart()
        {
            // Use the current authenticated user rather than requiring an id route value.
            var current = await _userManager.GetUserAsync(User);
            if (current == null)
            {
                // Not authenticated — redirect to login
                return Challenge();
            }

            var user = await _context.Users
                .Where(u => u.Id == current.Id)
                .Include(u => u.Orders!)
                    .ThenInclude(o => o.OrderItems!)
                        .ThenInclude(oi => oi.TicketType)
                .Include(u => u.Orders!)
                    .ThenInclude(o => o.Museum)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost, ActionName("Cart")]
        [Authorize(Roles = Roles.Client)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cart_Post()
        {
            // reuse GET behavior
            return await Cart();
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

        [HttpPost]
        [Authorize(Roles = Roles.Client)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelOrder(int orderId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Challenge();

            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.OrderId == orderId && o.UserId == currentUser.Id);

            if (order == null)
                return NotFound();

            if (order.Status != OrderStatus.New)
            {
                TempData["Error"] = "Only new orders can be cancelled.";
                return RedirectToAction(nameof(Cart));
            }

            if (order.VisitDate <= DateTime.Today)
            {
                TempData["Error"] = "You cannot cancel visits happening today or in the past.";
                return RedirectToAction(nameof(Cart));
            }

            order.Status = OrderStatus.Cancelled;
            await _context.SaveChangesAsync();

            TempData["Success"] = "Order cancelled successfully.";
            return RedirectToAction(nameof(Cart));
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

            return RedirectToAction(nameof(Dashboard));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var museum = await _context.Museums
                .FirstOrDefaultAsync(m => m.MuseumId == id);

            if (museum == null)
                return NotFound();

            return View(museum);
        }

        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MuseumModel museum)
        {
            ModelState.Remove(nameof(MuseumModel.TicketTypes));
            ModelState.Remove(nameof(MuseumModel.Orders));
            ModelState.Remove(nameof(MuseumModel.Employees));
            ModelState.Remove(nameof(MuseumModel.Images));

            // 🔥 ADD THESE
            ModelState.Remove(nameof(MuseumModel.OpeningTime));
            ModelState.Remove(nameof(MuseumModel.ClosingTime));

            if (!ModelState.IsValid)
            {
                return View(museum);
            }

            _context.Update(museum);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Dashboard));
        }

        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Accounts(string? search, int page = 1, int pageSize = 20)
        {
            ViewBag.Search = search ?? string.Empty;

            IQueryable<UserModel> usersQuery = _userManager.Users;

            if (!string.IsNullOrWhiteSpace(search))
            {
                var normalized = search.Trim();
                usersQuery = usersQuery.Where(u =>
                    EF.Functions.Like(u.UserName!, $"%{normalized}%") ||
                    EF.Functions.Like(u.Email!, $"%{normalized}%"));
            }

            var orderedQuery = usersQuery.AsNoTracking().OrderBy(u => u.Id);

            var totalCount = await orderedQuery.CountAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            if (totalPages == 0) totalPages = 1;
            page = Math.Clamp(page, 1, totalPages);

            var users = await orderedQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var model = new System.Collections.Generic.List<AccountViewModel>();
            foreach (var u in users)
            {
                var roles = await _userManager.GetRolesAsync(u);
                model.Add(new AccountViewModel
                {
                    Id = u.Id,
                    UserName = u.UserName ?? u.Email ?? "(n/a)",
                    Email = u.Email ?? "",
                    CurrentRole = roles.FirstOrDefault()
                });
            }

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.PageSize = pageSize;

            ViewBag.Roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateRole(int userId, string newRole, string? search, int page = 1)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) return NotFound();

            var currentRoles = await _userManager.GetRolesAsync(user);
            if (currentRoles.Any())
            {
                var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
                if (!removeResult.Succeeded)
                {
                    ModelState.AddModelError("", "Failed to remove existing roles.");
                }
            }

            if (!string.IsNullOrWhiteSpace(newRole))
            {
                var addResult = await _userManager.AddToRoleAsync(user, newRole);
                if (!addResult.Succeeded)
                {
                    ModelState.AddModelError("", "Failed to add role.");
                }
            }

            return RedirectToAction(nameof(Accounts), new { search, page });
        }

        public IActionResult Rules()
        {
            return View();
        }

        // Visit /Home/WhoAmI to see current identity and roles.
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
