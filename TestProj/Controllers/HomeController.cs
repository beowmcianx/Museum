using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text;
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

        public IActionResult Privacy() => View();

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
        public async Task<IActionResult> Create(MuseumModel museum)
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

            _context.Museums.Add(museum);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Dashboard(MuseumModel museum)
        {
            ModelState.Remove(nameof(MuseumModel.TicketTypes));
            ModelState.Remove(nameof(MuseumModel.Orders));
            ModelState.Remove(nameof(MuseumModel.Employees));
            ModelState.Remove(nameof(MuseumModel.Images));

            if (!ModelState.IsValid)
            {
                ViewBag.Museums = await _context.Museums.ToListAsync();
                return View("Dashboard", museum);
            }

            _context.Museums.Add(museum);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Catalogue()
        {
            var museums = await _context.Museums.ToListAsync();
            return View(museums);
        }

        public async Task<IActionResult> Details(int id)
        {
            var museum = await _context.Museums.FirstOrDefaultAsync(m => m.MuseumId == id);
            if (museum == null) return NotFound();
            return View(museum);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var museum = await _context.Museums.FirstOrDefaultAsync(m => m.MuseumId == id);
            if (museum == null) return NotFound();
            _context.Museums.Remove(museum);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MuseumModel museum)
        {
            ModelState.Remove(nameof(MuseumModel.TicketTypes));
            ModelState.Remove(nameof(MuseumModel.Orders));
            ModelState.Remove(nameof(MuseumModel.Employees));
            ModelState.Remove(nameof(MuseumModel.Images));

            if (!ModelState.IsValid)
            {
                return View(museum);
            }

            _context.Museums.Update(museum);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(int id)
        {
            var museum = await _context.Museums
                .FirstOrDefaultAsync(m => m.MuseumId == id);

            return View(museum);
        }

        public IActionResult WhoAmI()
        {
            var name = User.Identity?.Name ?? "(not authenticated)";
            var isAuthenticated = User.Identity?.IsAuthenticated ?? false;
            var roleClaims = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
            var rolesText = roleClaims.Any() ? string.Join(", ", roleClaims) : "(no role claims)";
            var claimsText = string.Join(" | ", User.Claims.Select(c => $"{c.Type}={c.Value}"));

            var text = $"Name: {name}\nAuthenticated: {isAuthenticated}\nRoles (cookie claims): {rolesText}\nClaims: {claimsText}";
            return Content(text, "text/plain");
        }

        public async Task<IActionResult> CheckAuth()
        {
            var sb = new StringBuilder();
            var principalName = User.Identity?.Name ?? "(not authenticated)";
            sb.AppendLine($"Principal.Name: {principalName}");
            sb.AppendLine($"Authenticated: {User.Identity?.IsAuthenticated ?? false}");

            var claimRoles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
            sb.AppendLine($"Roles from cookie claims: {(claimRoles.Any() ? string.Join(", ", claimRoles) : "(none)")}");

            UserModel? user = null;
            if (!string.IsNullOrEmpty(principalName))
            {
                user = await _userManager.FindByNameAsync(principalName) ?? await _userManager.FindByEmailAsync(principalName);
            }

            if (user == null)
            {
                sb.AppendLine("DB user not found for principal name. (Sign out & sign in after seeding.)");
                return Content(sb.ToString(), "text/plain");
            }

            sb.AppendLine($"DB user: Id={user.Id} UserName={user.UserName} Email={user.Email}");

            var rolesFromManager = await _userManager.GetRolesAsync(user);
            sb.AppendLine($"Roles from UserManager.GetRolesAsync: {(rolesFromManager.Any() ? string.Join(", ", rolesFromManager) : "(none)")}");

            var userRoleRows = await _context.UserRoles.Where(ur => ur.UserId == user.Id).ToListAsync();
            sb.AppendLine("AspNetUserRoles rows (UserId, RoleId):");
            foreach (var ur in userRoleRows)
            {
                sb.AppendLine($"  {ur.UserId}, {ur.RoleId}");
            }

            var rolesInDb = await _context.Roles.Select(r => new { r.Id, r.Name }).ToListAsync();
            sb.AppendLine("AspNetRoles rows (RoleId, Name):");
            foreach (var r in rolesInDb)
            {
                sb.AppendLine($"  {r.Id}, {r.Name}");
            }

            return Content(sb.ToString(), "text/plain");
        }
    }
}
