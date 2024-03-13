using ClubManagement.ApplicationDbContext;
using ClubManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClubManagement.Controllers
{
    public class PlanController : Controller
    {
        private readonly ClubDbContext _context;

        public PlanController(ClubDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult ShowPlan()
        {
            string userName = HttpContext.User.Identity.Name;
            var user = _context.Accounts.FirstOrDefault(u => u.AccountName == userName);
            if (user == null)
                return NotFound();

            if (user.Role == "Player")
            {
                return RedirectToAction("ShowPlanPlayer", new { id = user.FootballerId });
            }
            else if (user.Role == "Coach")
            {
                return View(); 
            }
            else
                return NotFound();
        }

        [HttpGet]
        public IActionResult ShowPlanPlayer(int? id) 
        {
            if(id == null || id == 0)
                return NotFound("ShowPlanPlayer brak id");

            var f = _context.Footballers
                .Include(f => f.IndividualTrainings)
                .Include(f => f.GroupTrainings)
                .FirstOrDefault(f => f.Id == id);

            return View(f);
        }

        public IActionResult ShowPlanCoach(Coach c)
        {
            if (c == null)
                return NotFound();

            return View(c);
        }
    }
}
