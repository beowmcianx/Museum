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
    public class WorkerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserModel> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;

        public WorkerController(
            ApplicationDbContext context,
            UserManager<UserModel> userManager,
            RoleManager<IdentityRole<int>> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Museum(int? id)
        {
            if (id == null) return NotFound();

            var museum = await _context.Museums
                .Include(m => m.Images)
                .Include(m => m.TicketTypes)
                .FirstOrDefaultAsync(m => m.MuseumId == id.Value);

            if (museum == null) return NotFound();

            return View(museum);
        }

        public async Task<IActionResult> EditMuseum(int? id)
        {
            if (id == null) return NotFound();

            var museum = await _context.Museums
                .Include(m => m.Images)
                .Include(m => m.TicketTypes)
                .FirstOrDefaultAsync(m => m.MuseumId == id.Value);

            if (museum == null) return NotFound();

            return View(museum);
        }

    }
}
