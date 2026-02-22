using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
            var imageUrl = Request.Form["ImageUrl"].ToString();
            if (!string.IsNullOrWhiteSpace(imageUrl))
            {
                museum.Images = museum.Images ?? new List<MuseumImageModel>();
                museum.Images.Add(new MuseumImageModel
                {
                    ImageUrl = imageUrl
                });
            }

            // Add the museum and related child entities (ticket types if you later add them)
            _context.Museums.Add(museum);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // Catalogue with pagination, filters and images included
        public async Task<IActionResult> Catalogue(string? searchString, string? cityFilter, string? typeFilter, int page = 1, int pageSize = 6)
        {
            // Preserve current filter/search values for the view
            ViewBag.SearchString = searchString ?? string.Empty;
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
                    EF.Functions.Like(m.City, s) ||
                    EF.Functions.Like(m.Type, s));
            }

            // Apply city/type filters
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

        // Dashboard now sets the same ViewBag values the view expects.
        // Supports filtering, paging and provides Cities/Types lists to avoid NRE in the view.
        public async Task<IActionResult> Dashboard(string? searchString, string? cityFilter, string? typeFilter, int page = 1, int pageSize = 10)
        {
            ViewBag.SearchString = searchString ?? string.Empty;
            ViewBag.CityFilter = cityFilter ?? string.Empty;
            ViewBag.TypeFilter = typeFilter ?? string.Empty;

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
                .AsNoTracking()
                .Include(m => m.Images);

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                var s = $"%{searchString.Trim()}%";
                query = query.Where(m =>
                    EF.Functions.Like(m.Name, s) ||
                    EF.Functions.Like(m.Description, s) ||
                    EF.Functions.Like(m.City, s) ||
                    EF.Functions.Like(m.Type, s));
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

            return RedirectToAction(nameof(Dashboard));
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

        // Update role POST
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
