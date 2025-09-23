using Accessio.Data;
using Accessio.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Accessio.Controllers
{
    [Authorize]
    public class GatePassesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GatePassesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: GatePasses
        public async Task<IActionResult> Index()
        {
            return View(await _context.GatePass.ToListAsync());
        }

        // GET: GatePasses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var gatePass = await _context.GatePass.FirstOrDefaultAsync(m => m.Id == id);
            if (gatePass == null) return NotFound();

            return View(gatePass);
        }

        // GET: GatePasses/Create
        public IActionResult Create()
        {
            PopulateRoleOptions();
            PopulateDepartmentOptions();
            return View();
        }

        // POST: GatePasses/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GatePass gatePass, IFormFile StudyLoadPdf, IFormFile RegistrationPdf)
        {
            if (ModelState.IsValid)
            {
                var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                if (!Directory.Exists(uploadDir))
                    Directory.CreateDirectory(uploadDir);

                if (StudyLoadPdf != null && StudyLoadPdf.Length > 0)
                {
                    var filePath = Path.Combine(uploadDir, StudyLoadPdf.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await StudyLoadPdf.CopyToAsync(stream);
                    }
                    gatePass.StudyLoadPdfPath = "/uploads/" + StudyLoadPdf.FileName;
                }

                if (RegistrationPdf != null && RegistrationPdf.Length > 0)
                {
                    var filePath = Path.Combine(uploadDir, RegistrationPdf.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await RegistrationPdf.CopyToAsync(stream);
                    }
                    gatePass.RegistrationPdfPath = "/uploads/" + RegistrationPdf.FileName;
                }

                _context.Add(gatePass);
                await _context.SaveChangesAsync();

                return Json(new { success = true });
            }

            PopulateRoleOptions(gatePass.Role);
            PopulateDepartmentOptions(gatePass.Department);

            return PartialView("_CreateForm", gatePass);
        }


        // GET: GatePasses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var gatePass = await _context.GatePass.FindAsync(id);
            if (gatePass == null) return NotFound();

            PopulateRoleOptions(gatePass.Role);
            PopulateDepartmentOptions(gatePass.Department);
            return View(gatePass);
        }

        // POST: GatePasses/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, GatePass gatePass, IFormFile? StudyLoadPdf, IFormFile? RegistrationPdf)
        {
            if (id != gatePass.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                    if (!Directory.Exists(uploadDir)) Directory.CreateDirectory(uploadDir);

                    // Fetch the existing entity from DB
                    var existingGatePass = await _context.GatePass.AsNoTracking().FirstOrDefaultAsync(g => g.Id == id);
                    if (existingGatePass == null) return NotFound();

                    // Keep previous PDF paths if no new file uploaded
                    gatePass.StudyLoadPdfPath = StudyLoadPdf != null && StudyLoadPdf.Length > 0
                        ? await SaveFileAsync(StudyLoadPdf, uploadDir)
                        : existingGatePass.StudyLoadPdfPath;

                    gatePass.RegistrationPdfPath = RegistrationPdf != null && RegistrationPdf.Length > 0
                        ? await SaveFileAsync(RegistrationPdf, uploadDir)
                        : existingGatePass.RegistrationPdfPath;

                    _context.Update(gatePass);
                    await _context.SaveChangesAsync();

                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                        return Json(new { success = true });

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GatePassExists(gatePass.Id)) return NotFound();
                    else throw;
                }
            }

            PopulateRoleOptions(gatePass.Role);
            PopulateDepartmentOptions(gatePass.Department);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return PartialView("_EditFormPartial", gatePass);

            return View(gatePass);
        }

        // Helper method to save uploaded file and return path
        private async Task<string> SaveFileAsync(IFormFile file, string uploadDir)
        {
            var filePath = Path.Combine(uploadDir, file.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return "/uploads/" + file.FileName;
        }



        // GET: GatePasses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var gatePass = await _context.GatePass.FirstOrDefaultAsync(m => m.Id == id);
            if (gatePass == null) return NotFound();

            return View(gatePass);
        }

        // POST: GatePasses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gatePass = await _context.GatePass.FindAsync(id);
            if (gatePass != null)
            {
                _context.GatePass.Remove(gatePass);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GatePassExists(int id)
        {
            return _context.GatePass.Any(e => e.Id == id);
        }

        
        private void PopulateRoleOptions(object? selectedValue = null)
        {
            ViewData["RoleOptions"] = Enum.GetValues(typeof(RoleOption))
                .Cast<RoleOption>()
                .Select(r => new SelectListItem
                {
                    Value = r.ToString(),
                    Text = r.GetType()
                            .GetMember(r.ToString())
                            .First()
                            .GetCustomAttributes(false)
                            .OfType<DisplayAttribute>()
                            .FirstOrDefault()?.Name ?? r.ToString()
                })
                .ToList();
        }

        private void PopulateDepartmentOptions(object? selectedValue = null)
        {
            ViewData["DepartmentOptions"] = Enum.GetValues(typeof(DepartmentOption))
                .Cast<DepartmentOption>()
                .Select(d => new SelectListItem
                {
                    Value = d.ToString(),
                    Text = d.GetType()
                            .GetMember(d.ToString())
                            .First()
                            .GetCustomAttributes(false)
                            .OfType<DisplayAttribute>()
                            .FirstOrDefault()?.Name ?? d.ToString()
                })
                .ToList();
        }
    }
}
