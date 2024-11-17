using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KASCFlightLog.Data;
using KASCFlightLog.Models;
using KASCFlightLog.Services;
using KASCFlightLog.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KASCFlightLog.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly INotificationService _notificationService;

        public DashboardController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            INotificationService notificationService)
        {
            _context = context;
            _userManager = userManager;
            _notificationService = notificationService;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains("Admin") || roles.Contains("Staff"))
            {
                return RedirectToAction(nameof(AdminDashboard));
            }

            return RedirectToAction(nameof(UserDashboard));
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
                    .Include(f => f.User)
                    .Where(f => !f.IsValid)
                    .ToListAsync(),

                RecentFlightLogs = await _context.FlightLogs
                    .Include(f => f.User)
                    .OrderByDescending(f => f.CreatedAt)
                    .Take(10)
                    .ToListAsync(),

                TotalUsers = await _context.Users.CountAsync(),
                TotalFlightLogs = await _context.FlightLogs.CountAsync(),
                PendingApprovals = await _context.FlightLogs.CountAsync(f => !f.IsValid),

                RecentNotifications = await _context.Notifications
                    .OrderByDescending(n => n.CreatedAt)
                    .Take(5)
                    .ToListAsync()
            };

            return View(viewModel);
        }

        [Authorize]
        public async Task<IActionResult> UserDashboard()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var myFlightLogs = await _context.FlightLogs
                .Where(f => f.UserId == user.Id)
                .OrderByDescending(f => f.CreatedAt)
                .ToListAsync();

            var viewModel = new UserDashboardViewModel
            {
                MyFlightLogs = myFlightLogs,
                PublishedFlightLogs = await _context.FlightLogs
                    .Where(f => f.IsPublished)
                    .Include(f => f.User)
                    .OrderByDescending(f => f.CreatedAt)
                    .Take(10)
                    .ToListAsync(),
                FullName = $"{user.FirstName} {user.LastName}",
                Email = user.Email,
                JoinDate = user.CreatedAt,
                TotalFlights = myFlightLogs.Count(),
                IncompleteFlightLogs = myFlightLogs.Count(f => f.Status == "Incomplete"),
                PartialFlightLogs = myFlightLogs.Count(f => f.Status == "Partial"),
                CompleteFlightLogs = myFlightLogs.Count(f => f.Status == "Complete"),
                RecentNotifications = await _context.Notifications
                    .Where(n => n.TargetUserId == user.Id)
                    .OrderByDescending(n => n.CreatedAt)
                    .Take(5)
                    .ToListAsync(),
                FlightsByAircraft = myFlightLogs
                    .GroupBy(f => f.RegistrationNO)
                    .ToDictionary(g => g.Key, g => g.Count()),
                FlightsByMonth = myFlightLogs
                    .GroupBy(f => f.FlightDate.ToString("MMMM yyyy"))
                    .ToDictionary(g => g.Key, g => g.Count())
            };

            return View(viewModel);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetNotifications()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var notifications = await _context.Notifications
                .Where(n => n.TargetUserId == user.Id && !n.IsRead)
                .OrderByDescending(n => n.CreatedAt)
                .Take(5)
                .ToListAsync();

            return Json(notifications);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> MarkNotificationAsRead(int notificationId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var notification = await _context.Notifications
                .FirstOrDefaultAsync(n => n.Id == notificationId && n.TargetUserId == user.Id);

            if (notification == null)
            {
                return NotFound();
            }

            notification.IsRead = true;
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}