using Accessio.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Accessio.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {

        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            int totalLockerRequests = _context.Locker.Count();
            int totalGatePassRequests = _context.GatePass.Count();

            ViewData["TotalLockers"] = totalLockerRequests;
            ViewData["TotalGatePasses"] = totalGatePassRequests;

            return View("Dashboard");
        }

    }
}