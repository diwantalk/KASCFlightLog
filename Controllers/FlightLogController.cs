using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using KASCFlightLog.Data;
using KASCFlightLog.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KASCFlightLog.Controllers
{
    [Authorize] // Requires authentication for all actions
    public class FlightLogsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FlightLogsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: FlightLogs
        public async Task<IActionResult> Index()
        {
            return View(await _context.FlightLogs
                .Include(f => f.Staff)
                .Include(f => f.User)
                .ToListAsync());
        }

        // GET: FlightLogs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flightLog = await _context.FlightLogs
                .Include(f => f.Staff)
                .Include(f => f.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (flightLog == null)
            {
                return NotFound();
            }

            return View(flightLog);
        }

        // GET: FlightLogs/Create
        [Authorize(Roles = "Admin,Staff")]
        public IActionResult Create()
        {
            ViewData["StaffId"] = new SelectList(_context.Staff, "Id", "Name");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name");
            return View();
        }

        // POST: FlightLogs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> Create([Bind("Id,Date,AircraftType,Registration,PIC,FI,Route,TimeOut,TimeIn,TimeDeparture,TimeArrival,SingleTime,DualTime,InstructionalTime,P1Time,P2Time,NightTime,IFRTime,UserId,StaffId")] FlightLog flightLog)
        {
            if (ModelState.IsValid)
            {
                _context.Add(flightLog);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["StaffId"] = new SelectList(_context.Staff, "Id", "Name", flightLog.StaffId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name", flightLog.UserId);
            return View(flightLog);
        }

        // GET: FlightLogs/Edit/5
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flightLog = await _context.FlightLogs.FindAsync(id);
            if (flightLog == null)
            {
                return NotFound();
            }
            ViewData["StaffId"] = new SelectList(_context.Staff, "Id", "Name", flightLog.StaffId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name", flightLog.UserId);
            return View(flightLog);
        }

        // POST: FlightLogs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,AircraftType,Registration,PIC,FI,Route,TimeOut,TimeIn,TimeDeparture,TimeArrival,SingleTime,DualTime,InstructionalTime,P1Time,P2Time,NightTime,IFRTime,UserId,StaffId")] FlightLog flightLog)
        {
            if (id != flightLog.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(flightLog);
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
            ViewData["StaffId"] = new SelectList(_context.Staff, "Id", "Name", flightLog.StaffId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name", flightLog.UserId);
            return View(flightLog);
        }

        // GET: FlightLogs/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flightLog = await _context.FlightLogs
                .Include(f => f.Staff)
                .Include(f => f.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (flightLog == null)
            {
                return NotFound();
            }

            return View(flightLog);
        }

        // POST: FlightLogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var flightLog = await _context.FlightLogs.FindAsync(id);
            _context.FlightLogs.Remove(flightLog);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FlightLogExists(int id)
        {
            return _context.FlightLogs.Any(e => e.Id == id);
        }
    }
}