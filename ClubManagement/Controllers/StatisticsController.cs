using ClubManagement.ApplicationDbContext;
using ClubManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClubManagement.Controllers
{
    public class StatisticsController : Controller
    {
        private readonly ClubDbContext _context;

        public StatisticsController(ClubDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        // Edit Statistics
        [HttpGet]
        [Authorize(Policy = "AdminAccess")]
        [Authorize(Policy = "CoachAccess")]
        public IActionResult EditStatistics(int? id)
        {
            if(id == null || id == 0)
                return NotFound();

            var obj = _context.Statistics.FirstOrDefault(x => x.Id == id);

            if(obj == null)
                return NotFound();

            return View(obj);
        }

        [HttpPost]
        [Authorize(Policy = "AdminAccess")]
        [Authorize(Policy = "CoachAccess")]
        public IActionResult EditStatistics([FromForm]Statistics statistics)
        {
            _context.Statistics.Update(statistics);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
