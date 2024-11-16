using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using KASCFlightLog.Data;
using KASCFlightLog.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using KASCFlightLog.ViewModels;
using KASCFlightLog.Services;


namespace KASCFlightLog.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DashboardController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains("Admin") || roles.Contains("Staff"))
            {
                return RedirectToAction("AdminDashboard");
            }

            return RedirectToAction("UserDashboard");
        }

        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> AdminDashboard()
        {
            var viewModel = new AdminDashboardViewModel
            {
                PendingUsers = await _context.Users
                    .Where(u => !u.IsValidated)
                    .ToListAsync(),

                PendingFlightLogs = await _context.FlightLogs
                    .Include(f => f.UserId)
                    .Where(f => f.Status == "Pending")
                    .ToListAsync(),

                RecentNotifications = await _context.Notifications
                    .Where(n => !n.IsRead)
                    .OrderByDescending(n => n.CreatedAt)
                    .Take(5)
                    .ToListAsync()
            };

            return View(viewModel);
        }

        [Authorize(Roles = "RegularUser")]
        public async Task<IActionResult> UserDashboard()
        {
            var user = await _userManager.GetUserAsync(User);
            var viewModel = new UserDashboardViewModel
            {
                MyFlightLogs = await _context.FlightLogs
                    .Where(f => f.UserId == user.Id)
                    .OrderByDescending(f => f.CreatedAt)
                    .Take(10)
                    .ToListAsync(),

                PublishedFlightLogs = await _context.FlightLogs
                    .Include(f => f.UserId)
                    .Where(f => f.Status == "Published")
                    .OrderByDescending(f => f.CreatedAt)
                    .Take(10)
                    .ToListAsync()
            };

            return View(viewModel);
        }
    }
}