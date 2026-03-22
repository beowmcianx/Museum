using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;
using TestProj.Data;
using TestProj.Models;

namespace TestProj.Controllers
{
    [Authorize(Roles = "Worker")]
    public class WorkerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserModel> _userManager;

        public WorkerController(ApplicationDbContext context, UserManager<UserModel> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Shows the Worker dashboard for their assigned museum
        public async Task<IActionResult> MyMuseum()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var employee = await _context.MuseumEmployees
                                         .Include(e => e.Museum)
                                         .FirstOrDefaultAsync(e => e.UserId == userId);

            if (employee?.MuseumId == null || employee.Museum == null)
            {
                TempData["Error"] = "You are not assigned to a museum.";
                return RedirectToAction("Index", "Home");
            }

            return View(employee.Museum); // Pass MuseumModel to the view
        }

        // Edit museum page
        public async Task<IActionResult> EditMuseum(int? id)
        {
            if (id == null)
            {
                TempData["Error"] = "Museum ID is missing.";
                return RedirectToAction(nameof(MyMuseum));
            }

            var museum = await _context.Museums
                .Include(m => m.Images)
                .Include(m => m.TicketTypes)
                .FirstOrDefaultAsync(m => m.MuseumId == id.Value);

            if (museum == null)
            {
                TempData["Error"] = "Museum could not be found.";
                return RedirectToAction(nameof(MyMuseum));
            }

            return View(museum);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditMuseum(MuseumModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var museum = await _context.Museums
                .Include(m => m.TicketTypes)
                .FirstOrDefaultAsync(m => m.MuseumId == model.MuseumId);

            if (museum == null)
            {
                TempData["Error"] = "Museum could not be found.";
                return RedirectToAction(nameof(MyMuseum));
            }

            museum.Name = model.Name;
            museum.Country = model.Country;
            museum.City = model.City;
            museum.Address = model.Address;
            museum.Description = model.Description;
            museum.OpeningTime = model.OpeningTime;
            museum.ClosingTime = model.ClosingTime;
            museum.Type = model.Type;

            await _context.SaveChangesAsync();

            TempData["Success"] = "Museum updated successfully!";
            return RedirectToAction(nameof(EditMuseum), new { id = museum.MuseumId });
        }

        public async Task<IActionResult> MuseumOrders(DateTime? selectedDate)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var employee = await _context.MuseumEmployees
                .Include(e => e.Museum)
                    .ThenInclude(m => m.Orders)
                        .ThenInclude(o => o.User)
                .Include(e => e.Museum)
                    .ThenInclude(m => m.Orders)
                        .ThenInclude(o => o.OrderItems)
                .FirstOrDefaultAsync(e => e.UserId == userId);

            if (employee?.Museum == null)
            {
                TempData["Error"] = "You are not assigned to a museum.";
                return RedirectToAction("Index", "Home");
            }

            // ✅ If date selected → filter
            if (selectedDate.HasValue)
            {
                employee.Museum.Orders = employee.Museum.Orders
                    .Where(o => o.VisitDate.Date == selectedDate.Value.Date)
                    .ToList();
            }
            // ❗ If NOT selected → do nothing (shows ALL)

            ViewBag.SelectedDate = selectedDate;

            return View(employee.Museum);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, OrderStatus status)
        {
            var order = await _context.Orders.FindAsync(orderId);

            if (order == null)
            {
                return NotFound();
            }

            if (order.Status == status)
            {
                return BadRequest("Order already has this status.");
            }

            if (order.Status == OrderStatus.Cancelled)
            {
                return BadRequest("Cancelled orders cannot be modified.");
            }

            if (order.Status == OrderStatus.Used)
            {
                return BadRequest("Order already used.");
            }

            if (status == OrderStatus.Used && order.Status != OrderStatus.Confirmed)
            {
                return BadRequest("Only confirmed orders can be used.");
            }

            order.Status = status;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(MuseumOrders), new { selectedDate = order.VisitDate });
        }
    }
}