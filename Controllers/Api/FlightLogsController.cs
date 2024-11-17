using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using KASCFlightLog.Data;
using KASCFlightLog.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using KASCFlightLog.Services;
//using KASCFlightLog.DTOs;

namespace KASCFlightLog.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FlightLogsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly INotificationService _notificationService;

        public FlightLogsController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            INotificationService notificationService)
        {
            _context = context;
            _userManager = userManager;
            _notificationService = notificationService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateFlightLog([FromBody] FlightLog dto)
        {
            var user = await _userManager.GetUserAsync(User);

            var flightLog = new FlightLog
            {
                UserId = user.Id,
                FlightDate = dto.FlightDate,
                RegistrationNO = dto.RegistrationNO,
                From = dto.From,
                To = dto.To,
                //Status = dto.Status,
                CreatedAt = DateTime.UtcNow
            };

            _context.FlightLogs.Add(flightLog);
            await _context.SaveChangesAsync();

            // Notify admins and staff
            await _notificationService.NotifyAdminsAndStaff(
                "New Flight Log",
                $"New flight log submitted by {user.FirstName} {user.LastName}"
            );

            return Ok(flightLog);
        }

        [HttpPost("{id}/approve")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> ApproveFlightLog(int id)
        {
            var flightLog = await _context.FlightLogs
                .Include(f => f.User)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (flightLog == null)
                return NotFound();

            //flightLog.Status = "Published";
            flightLog.PublishedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            // Notify the pilot
            await _notificationService.NotifyUser(
                flightLog.UserId,
                "Flight Log Approved",
                "Your flight log has been approved and published."
            );

            return Ok();
        }

        [HttpPost("{id}/reject")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> RejectFlightLog(int id)
        {
            var flightLog = await _context.FlightLogs
                .Include(f => f.User)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (flightLog == null)
                return NotFound();

            //flightLog.Status = "Rejected";
            flightLog.LastModifiedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            // Notify the pilot
            await _notificationService.NotifyUser(
                flightLog.UserId,
                "Flight Log Rejected",
                "Your flight log has been rejected. Please contact staff for more information."
            );

            return Ok();
        }
    }
}