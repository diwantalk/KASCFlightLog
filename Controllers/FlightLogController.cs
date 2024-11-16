using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KASCFlightLog.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using KASCFlightLog.Data;

namespace KASCFlightLog.Controllers
{
    [Authorize]
    public class FlightLogController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public FlightLogController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: FlightLog
        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var isAdminOrStaff = User.IsInRole("Admin") || User.IsInRole("Staff");

            // If admin or staff, show all logs, otherwise show only user's logs
            var flightLogs = isAdminOrStaff
                ? await _context.FlightLogs.Include(f => f.User).Include(f => f.ValidatedBy).ToListAsync()
                : await _context.FlightLogs.Where(f => f.UserId == currentUser.Id).Include(f => f.User).Include(f => f.ValidatedBy).ToListAsync();

            return View(flightLogs);
        }

        // POST: FlightLog/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RegistrationNO,PilotInCommand,P3PAX,From,To,AuthorizedBy,Remarks,TimeDeparture,TimeArrival,NumberOfLandings,Note")] FlightLog flightLog)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await _userManager.GetUserAsync(User);
                flightLog.UserId = currentUser.Id;
                flightLog.CreatedBy = currentUser.Id;
                flightLog.CreatedAt = DateTime.UtcNow;

                // If user is not staff/admin, automatically set pilot name
                if (!User.IsInRole("Admin") && !User.IsInRole("Staff"))
                {
                    flightLog.PilotInCommand = $"{currentUser.FirstName} {currentUser.LastName}";
                }

                // Calculate duration
                if (flightLog.TimeDeparture.HasValue && flightLog.TimeArrival.HasValue)
                {
                    flightLog.CalculateDuration();
                }

                _context.Add(flightLog);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(flightLog);
        }

        // POST: FlightLog/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RegistrationNO,PilotInCommand,P3PAX,From,To,AuthorizedBy,Remarks,TimeDeparture,TimeArrival,NumberOfLandings,Note,IsValid,IsPublished")] FlightLog flightLog)
        {
            if (id != flightLog.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var currentUser = await _userManager.GetUserAsync(User);
                    var existingLog = await _context.FlightLogs.FindAsync(id);

                    if (existingLog == null)
                    {
                        return NotFound();
                    }

                    // Update properties
                    existingLog.LastModifiedAt = DateTime.UtcNow;
                    existingLog.LastModifiedBy = currentUser.Id;

                    // Update validation status if user is staff/admin
                    if (User.IsInRole("Admin") || User.IsInRole("Staff"))
                    {
                        existingLog.IsValid = flightLog.IsValid;
                        existingLog.IsPublished = flightLog.IsPublished;
                        if (flightLog.IsValid && existingLog.ValidatedById == null)
                        {
                            existingLog.ValidatedById = currentUser.Id;
                            existingLog.ValidatedAt = DateTime.UtcNow;
                        }
                    }

                    // Update other properties
                    existingLog.RegistrationNO = flightLog.RegistrationNO;
                    existingLog.PilotInCommand = flightLog.PilotInCommand;
                    existingLog.P3PAX = flightLog.P3PAX;
                    existingLog.From = flightLog.From;
                    existingLog.To = flightLog.To;
                    existingLog.AuthorizedBy = flightLog.AuthorizedBy;
                    existingLog.Remarks = flightLog.Remarks;
                    existingLog.TimeDeparture = flightLog.TimeDeparture;
                    existingLog.TimeArrival = flightLog.TimeArrival;
                    existingLog.NumberOfLandings = flightLog.NumberOfLandings;
                    existingLog.Note = flightLog.Note;

                    // Recalculate duration
                    if (existingLog.TimeDeparture.HasValue && existingLog.TimeArrival.HasValue)
                    {
                        existingLog.CalculateDuration();
                    }

                    _context.Update(existingLog);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FlightLogExists(flightLog.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(flightLog);
        }

        private bool FlightLogExists(int id)
        {
            return _context.FlightLogs.Any(e => e.Id == id);
        }
    }
}