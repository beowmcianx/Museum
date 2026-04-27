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
using TestProj.Data.Services;
using System.Text.Json;

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

        [HttpGet]
        [Authorize(Roles = Roles.Client)]
        public async Task<IActionResult> TicketQr(int ticketId)
        {
            // Ensure the requester is authenticated
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Challenge();

            // Load the order item and related data
            var item = await _context.OrderItems
                .Include(oi => oi.Order)
                    .ThenInclude(o => o.Museum)
                .Include(oi => oi.TicketType)
                .AsNoTracking()
                .FirstOrDefaultAsync(oi => oi.OrderItemId == ticketId);

            if (item == null || item.Order == null) return NotFound();

            // Ensure the current user owns this order item
            if (item.Order.UserId != currentUser.Id)
                return Forbid();

            // Build a compact payload containing all relevant ticket information
            var readable = $@"Order: {item.Order?.OrderCode}
                Ticket: {item.TicketType?.Name}
                Quantity: {item.Quantity}
                Price: {item.PriceAtPurchase:C}
                Visit: {item.Order?.VisitDate:yyyy-MM-dd}
                Museum: {item.Order?.Museum?.Name} ({item.Order?.Museum?.City})
                Details: {Url.Action("TicketDetails", "Client", new { id = item.OrderItemId }, Request.Scheme, Request.Host.Value)}
                ";
            var qrBytes = QrHelper.Generate(readable);

            return File(qrBytes, "image/png");
        }

        [HttpGet]
        [Authorize(Roles = Roles.Client)]
        public async Task<IActionResult> TicketDetails(int id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Challenge();

            var item = await _context.OrderItems
                .Include(oi => oi.Order)
                    .ThenInclude(o => o.Museum)
                .Include(oi => oi.TicketType)
                .AsNoTracking()
                .FirstOrDefaultAsync(oi => oi.OrderItemId == id);

            if (item == null || item.Order == null) return NotFound();

            if (item.Order.UserId != currentUser.Id)
                return Forbid();

            return View(item);
        }
    }
}
