using Accessio.Data;
using Accessio.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Accessio.Controllers
{
    [Authorize]
    public class LockersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LockersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Lockers
        public async Task<IActionResult> Index()
        {
            return View(await _context.Locker.ToListAsync());
        }

        // GET: Lockers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var locker = await _context.Locker
                .FirstOrDefaultAsync(m => m.Id == id);
            if (locker == null)
            {
                return NotFound();
            }

            return View(locker);
        }

        // GET: Lockers/Create
        public IActionResult Create()
        {
            ViewData["SemesterOptions"] = Enum.GetValues(typeof(SemesterOption))
                .Cast<SemesterOption>()
                .Select(s => new SelectListItem
                {
                    Value = s.ToString(),
                    Text = s.GetType()
                            .GetMember(s.ToString())
                            .First()
                            .GetCustomAttributes(false)
                            .OfType<DisplayAttribute>()
                            .FirstOrDefault()?.Name ?? s.ToString()
                })
                .ToList();

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Locker locker, IFormFile StudyLoadPdfPath)
        {
            if (ModelState.IsValid)
            {
                if (StudyLoadPdfPath != null && StudyLoadPdfPath.Length > 0)
                {
                    var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                    if (!Directory.Exists(uploadDir))
                        Directory.CreateDirectory(uploadDir);

                    var filePath = Path.Combine(uploadDir, StudyLoadPdfPath.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await StudyLoadPdfPath.CopyToAsync(stream);
                    }

                    locker.StudyLoadPdfPath = "/uploads/" + StudyLoadPdfPath.FileName;
                }

                _context.Add(locker);
                await _context.SaveChangesAsync();

                return Json(new { success = true });
            }

            ViewData["SemesterOptions"] = new SelectList(Enum.GetValues(typeof(SemesterOption)));
            return PartialView("_CreateForm", locker);
        }



        // GET: Lockers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var locker = await _context.Locker.FindAsync(id);
            if (locker == null)
            {
                return NotFound();
            }

            PopulateSemesterOptions(locker.Semester);
            return View(locker);
        }

        // POST: Lockers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Locker locker, IFormFile? StudyLoadPdfPath)
        {
            if (id != locker.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingLocker = await _context.Locker.AsNoTracking().FirstOrDefaultAsync(l => l.Id == id);
                    if (existingLocker == null)
                    {
                        return NotFound();
                    }

                    if (StudyLoadPdfPath != null && StudyLoadPdfPath.Length > 0)
                    {
                        var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                        if (!Directory.Exists(uploadDir))
                            Directory.CreateDirectory(uploadDir);

                        var fileName = Path.GetFileName(StudyLoadPdfPath.FileName);
                        var filePath = Path.Combine(uploadDir, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await StudyLoadPdfPath.CopyToAsync(stream);
                        }

                        locker.StudyLoadPdfPath = "/uploads/" + fileName;
                    }
                    else
                    {
                        locker.StudyLoadPdfPath = existingLocker.StudyLoadPdfPath;
                    }

                    _context.Update(locker);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LockerExists(locker.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            PopulateSemesterOptions(locker.Semester);
            return View(locker);
        }


        // GET: Lockers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var locker = await _context.Locker
                .FirstOrDefaultAsync(m => m.Id == id);
            if (locker == null)
            {
                return NotFound();
            }

            return View(locker);
        }

        // POST: Lockers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var locker = await _context.Locker.FindAsync(id);
            if (locker != null)
            {
                _context.Locker.Remove(locker);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LockerExists(int id)
        {
            return _context.Locker.Any(e => e.Id == id);
        }

        // Helper method to populate Semester dropdown
        private void PopulateSemesterOptions(object? selectedValue = null)
        {
            ViewData["SemesterOptions"] = new SelectList(Enum.GetValues(typeof(SemesterOption)), selectedValue);
        }
    }
}
