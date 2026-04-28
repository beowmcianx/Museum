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
using Museum.Data;
using Museum.Models;
using Museum.Models.NewFolder;
using Museum.Models.ViewModels;
using Museum.Data.Services;
using System.Text.Json;

namespace Museum.Controllers
{
    public class ClientController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserModel> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly IEmailService _emailService;

        public ClientController(
            ApplicationDbContext context,
            UserManager<UserModel> userManager,
            RoleManager<IdentityRole<int>> roleManager,
            IEmailService emailService)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _emailService = emailService;
        }

        [HttpGet]
        [Authorize(Roles = Roles.Client)]
        public async Task<IActionResult> Cart()
        {
            var current = await _userManager.GetUserAsync(User);
            if (current == null)
            {
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
                .Include(o => o.Museum)
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

            var oldStatus = order.Status;
            order.Status = OrderStatus.Cancelled;
            await _context.SaveChangesAsync();

            // Send cancellation email
            try
            {
                await _emailService.SendOrderStatusChangeAsync(
                    order,
                    currentUser.Email,
                    currentUser.FullName,
                    OrderStatus.Cancelled);
            }
            catch (Exception ex)
            {
                // Log but don't fail the request
                Console.WriteLine($"Email send failed: {ex.Message}");
            }

            TempData["Success"] = "Order cancelled successfully. A confirmation email has been sent.";
            return RedirectToAction(nameof(Cart));
        }

        [HttpGet]
        [Authorize(Roles = Roles.Client)]
        public async Task<IActionResult> TicketQr(int ticketId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Challenge();

            var item = await _context.OrderItems
                .Include(oi => oi.Order)
                    .ThenInclude(o => o.Museum)
                .Include(oi => oi.TicketType)
                .AsNoTracking()
                .FirstOrDefaultAsync(oi => oi.OrderItemId == ticketId);

            if (item == null || item.Order == null) return NotFound();

            if (item.Order.UserId != currentUser.Id)
                return Forbid();

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

        // Call this method after order creation
        public async Task SendOrderConfirmationEmailAsync(OrderModel order, UserModel user)
        {
            try
            {
                var orderItems = await _context.OrderItems
                    .Where(oi => oi.OrderId == order.OrderId)
                    .Include(oi => oi.TicketType)
                    .ToListAsync();

                var tickets = new List<(string, int, byte[])>();

                foreach (var item in orderItems)
                {
                    var readable = $@"Order: {order.OrderCode}
                        Ticket: {item.TicketType.Name}
                        Quantity: {item.Quantity}
                        Price: {item.PriceAtPurchase:C}
                        Visit: {order.VisitDate:yyyy-MM-dd}
                        Museum: {order.Museum.Name} ({order.Museum.City})";

                    var qrCode = QrHelper.Generate(readable);
                    tickets.Add((item.TicketType.Name, item.Quantity, qrCode));
                }

                await _emailService.SendOrderConfirmationAsync(
                    order,
                    user.Email,
                    user.FullName,
                    tickets);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Email send failed: {ex.Message}");
            }
        }
    }
}
