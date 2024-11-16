using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using KASCFlightLog.Data;
using KASCFlightLog.Models;
using Microsoft.AspNetCore.Identity;
using KASCFlightLog.Services;

namespace KASCFlightLog.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Staff")]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly INotificationService _notificationService;

        public UsersController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            INotificationService notificationService)
        {
            _context = context;
            _userManager = userManager;
            _notificationService = notificationService;
        }

        [HttpPost("{id}/approve")]
        public async Task<IActionResult> ApproveUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return NotFound();

            user.IsValidated = true;
            await _userManager.UpdateAsync(user);

            // Assign default role
            await _userManager.AddToRoleAsync(user, "RegularUser");

            // Notify the user
            await _notificationService.NotifyUser(
                user.Id,
                "Account Approved",
                "Your account has been approved. You can now log in."
            );

            return Ok();
        }

        [HttpPost("{id}/reject")]
        public async Task<IActionResult> RejectUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return NotFound();

            // Store email for notification before deletion
            var userEmail = user.Email;

            // Delete the user
            await _userManager.DeleteAsync(user);

            // Send email notification
            // TODO: Implement email service

            return Ok();
        }
    }
}