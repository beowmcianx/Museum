using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using TestProj.Data;
using TestProj.Models;
using TestProj.Models.NewFolder;

namespace TestProj.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserModel> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;

        public DashboardController(
            ApplicationDbContext context,
            UserManager<UserModel> userManager,
            RoleManager<IdentityRole<int>> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
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

            // Base query (include images)
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

        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Create()
        {
            ViewBag.Museums = await _context.Museums.ToListAsync();

            return View(new MuseumModel());
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

            _context.Museums.Add(museum);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Dashboard), "Dashboard");
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
                .Include(m => m.TicketTypes)
                .Include(m => m.Images)
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
            var existingMuseum = await _context.Museums
                .Include(m => m.TicketTypes)
                .Include(m => m.Images)
                .FirstOrDefaultAsync(m => m.MuseumId == museum.MuseumId);

            if (existingMuseum == null)
                return NotFound();

            // Update basic fields
            existingMuseum.Name = museum.Name;
            existingMuseum.Country = museum.Country;
            existingMuseum.City = museum.City;
            existingMuseum.Address = museum.Address;
            existingMuseum.Description = museum.Description;
            existingMuseum.Type = museum.Type;
            existingMuseum.OpeningTime = museum.OpeningTime;
            existingMuseum.ClosingTime = museum.ClosingTime;

            // ----------------------
            // UPDATE IMAGE
            // ----------------------

            var imageUrl = Request.Form["ImageUrl"].ToString();

            if (!string.IsNullOrWhiteSpace(imageUrl))
            {
                if (existingMuseum.Images == null || !existingMuseum.Images.Any())
                {
                    existingMuseum.Images = new List<MuseumImageModel>
            {
                new MuseumImageModel
                {
                    ImageUrl = imageUrl,
                    MuseumId = existingMuseum.MuseumId
                }
            };
                }
                else
                {
                    existingMuseum.Images.First().ImageUrl = imageUrl;
                }
            }

            // ----------------------
            // UPDATE TICKET TYPES
            // ----------------------

            var ticketKeys = Request.Form.Keys
                .Where(k => k.StartsWith("TicketTypes["))
                .ToList();

            if (ticketKeys.Any())
            {
                // Remove old tickets
                _context.TicketTypes.RemoveRange(existingMuseum.TicketTypes);

                existingMuseum.TicketTypes = new List<TicketTypeModel>();

                var indices = ticketKeys
                    .Select(k =>
                    {
                        var start = k.IndexOf('[');
                        var end = k.IndexOf(']');
                        if (start >= 0 && end > start)
                            return k.Substring(start + 1, end - start - 1);
                        return null;
                    })
                    .Where(x => x != null)
                    .Distinct();

                foreach (var idx in indices)
                {
                    var name = Request.Form[$"TicketTypes[{idx}].Name"];
                    var priceStr = Request.Form[$"TicketTypes[{idx}].Price"];

                    if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(priceStr))
                        continue;

                    if (!decimal.TryParse(priceStr, out var price))
                        continue;

                    existingMuseum.TicketTypes.Add(new TicketTypeModel
                    {
                        Name = name,
                        Price = price,
                        MuseumId = existingMuseum.MuseumId
                    });
                }
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Dashboard));
        }

        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Analytics()
        {
            return View();
        }

        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Accounts(string? search, string? roleFilter, int? museumFilter, int page = 1, int pageSize = 20)
        {
            ViewBag.Search = search ?? "";
            ViewBag.RoleFilter = roleFilter ?? "";
            ViewBag.MuseumFilter = museumFilter?.ToString() ?? "";

            // Load museums for dropdown
            var museums = await _context.Museums
                .AsNoTracking()
                .OrderBy(m => m.Name)
                .ToListAsync();

            ViewBag.Museums = museums;

            IQueryable<UserModel> usersQuery = _userManager.Users.AsNoTracking();

            // Search filter
            if (!string.IsNullOrWhiteSpace(search))
            {
                var normalized = search.Trim();
                usersQuery = usersQuery.Where(u =>
                    EF.Functions.Like(u.UserName!, $"%{normalized}%") ||
                    EF.Functions.Like(u.Email!, $"%{normalized}%"));
            }

            // Convert to list first to get roles & museum info
            var usersList = await usersQuery
                .OrderBy(u => u.Id)
                .ToListAsync();

            var model = new List<AccountViewModel>();

            foreach (var u in usersList)
            {
                var roles = await _userManager.GetRolesAsync(u);

                var employee = await _context.MuseumEmployees
                    .AsNoTracking()
                    .FirstOrDefaultAsync(e => e.UserId == u.Id);

                model.Add(new AccountViewModel
                {
                    Id = u.Id,
                    UserName = u.UserName ?? u.Email ?? "(n/a)",
                    Email = u.Email ?? "",
                    CurrentRole = roles.FirstOrDefault(),
                    AssignedMuseumId = employee?.MuseumId
                });
            }

            // Apply role filter
            if (!string.IsNullOrWhiteSpace(roleFilter))
            {
                model = model.Where(a => a.CurrentRole == roleFilter).ToList();
            }

            // Apply museum filter
            if (museumFilter.HasValue)
            {
                model = model.Where(a => a.AssignedMuseumId == museumFilter.Value).ToList();
            }

            // Pagination
            var totalCount = model.Count;
            var totalPages = Math.Max(1, (int)Math.Ceiling(totalCount / (double)pageSize));
            page = Math.Clamp(page, 1, totalPages);

            model = model.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.PageSize = pageSize;

            // Roles dropdown
            ViewBag.Roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateRole(int userId, string? newRole, int? museumId, string? search, string? roleFilter, int? museumFilter, int page = 1)
        {
            var user = await _context.Users
                .Include(u => u.MuseumEmployee)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null) return NotFound();

            // Update roles
            var currentRoles = await _userManager.GetRolesAsync(user);
            if (currentRoles.Any())
                await _userManager.RemoveFromRolesAsync(user, currentRoles);

            if (!string.IsNullOrWhiteSpace(newRole))
                await _userManager.AddToRoleAsync(user, newRole);

            // Update museum assignment
            if (newRole != Roles.Worker)
            {
                if (user.MuseumEmployee != null)
                    _context.MuseumEmployees.Remove(user.MuseumEmployee);
            }
            else
            {
                if (museumId == null)
                {
                    if (user.MuseumEmployee != null)
                        _context.MuseumEmployees.Remove(user.MuseumEmployee);
                }
                else
                {
                    if (user.MuseumEmployee == null)
                    {
                        _context.MuseumEmployees.Add(new MuseumEmployeeModel
                        {
                            UserId = userId,
                            MuseumId = museumId.Value
                        });
                    }
                    else
                    {
                        user.MuseumEmployee.MuseumId = museumId.Value;
                    }
                }
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Accounts), new { search, roleFilter, museumFilter, page });
        }
    }
}
