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
    public class ClientController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserModel> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;

        public ClientController(
            ApplicationDbContext context,
            UserManager<UserModel> userManager,
            RoleManager<IdentityRole<int>> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
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
            return await Cart();
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
    }
}
