using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Accessio.Data;
using Accessio.Models;

namespace Accessio.Controllers
{
    public class ActivityReservationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ActivityReservationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ActivityReservations
        public async Task<IActionResult> Index()
        {
            return View(await _context.ActivityReservation.ToListAsync());
        }

        // GET: ActivityReservations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activityReservation = await _context.ActivityReservation
                .FirstOrDefaultAsync(m => m.Id == id);
            if (activityReservation == null)
            {
                return NotFound();
            }

            return View(activityReservation);
        }

        // GET: ActivityReservations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ActivityReservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Department,ApplicationDate,ActivityTitle,SpeakerName,Venue,PurposeObjective,EventEquipmentRequest,DateNeeded,TimeFrom,TimeTo,Participants,NatureOfActivity,SourceOfFunds")] ActivityReservation activityReservation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(activityReservation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(activityReservation);
        }

        // GET: ActivityReservations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var activityReservation = await _context.ActivityReservation.FindAsync(id);
            if (activityReservation == null)
                return NotFound();

            PopulateDepartmentOptions(activityReservation.Department);
            return View(activityReservation);
        }

        // POST: ActivityReservations/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Department,ApplicationDate,ActivityTitle,SpeakerName,Venue,PurposeObjective,EventEquipmentRequest,DateNeeded,TimeFrom,TimeTo,Participants,NatureOfActivity,SourceOfFunds")] ActivityReservation activityReservation)
        {
            if (id != activityReservation.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(activityReservation);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActivityReservationExists(activityReservation.Id))
                        return NotFound();
                    else
                        throw;
                }
            }

            // Repopulate dropdown if validation fails
            PopulateDepartmentOptions(activityReservation.Department);
            return View(activityReservation);
        }

        // Helper for Department dropdown
        private void PopulateDepartmentOptions(object? selectedValue = null)
        {
            ViewData["DepartmentOptions"] = new SelectList(
                Enum.GetValues(typeof(DepartmentOption)),
                selectedValue
            );
        }


        // GET: ActivityReservations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activityReservation = await _context.ActivityReservation
                .FirstOrDefaultAsync(m => m.Id == id);
            if (activityReservation == null)
            {
                return NotFound();
            }

            return View(activityReservation);
        }

        // POST: ActivityReservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var activityReservation = await _context.ActivityReservation.FindAsync(id);
            if (activityReservation != null)
            {
                _context.ActivityReservation.Remove(activityReservation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ActivityReservationExists(int id)
        {
            return _context.ActivityReservation.Any(e => e.Id == id);
        }
    }
}
