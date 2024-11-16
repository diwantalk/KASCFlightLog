using KASCFlightLog.Data;
using KASCFlightLog.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace KASCFlightLog.Services
{
    public class NotificationService : INotificationService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationService(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            IHubContext<NotificationHub> hubContext)
        {
            _context = context;
            _userManager = userManager;
            _hubContext = hubContext;
        }

        public async Task NotifyUser(string userId, string title, string message)
        {
            var notification = new Notification
            {
                Type = title,
                Message = message,
                CreatedAt = DateTime.UtcNow,
                TargetUserId = userId,
                IsRead = false
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();

            // Send real-time notification
            await _hubContext.Clients.User(userId)
                .SendAsync("ReceiveNotification", new { title, message });
        }

        public async Task NotifyAdminsAndStaff(string title, string message)
        {
            var adminsAndStaff = await _userManager.GetUsersInRoleAsync("Admin");
            adminsAndStaff.Union(await _userManager.GetUsersInRoleAsync("Staff"));

            foreach (var user in adminsAndStaff)
            {
                await NotifyUser(user.Id, title, message);
            }
        }

        public async Task<IEnumerable<Notification>> GetUnreadNotifications(string userId)
        {
            return await _context.Notifications
                .Where(n => n.TargetUserId == userId && !n.IsRead)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task MarkAsRead(int notificationId)
        {
            var notification = await _context.Notifications.FindAsync(notificationId);
            if (notification != null)
            {
                notification.IsRead = true;
                await _context.SaveChangesAsync();
            }
        }
    }
}