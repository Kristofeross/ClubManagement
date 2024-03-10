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

        [HttpGet]
        public IActionResult ShowStatistics()
        {
            IEnumerable<Footballer> statistics = _context.Footballers
                .Include(x => x.Statistics).ToList();

            return View(statistics);
        }

        // Edit Statistics
        [HttpGet]
        [Authorize(Policy = "AdminAccess")]
        //[Authorize(Policy = "CoachAccess")]
        public IActionResult EditStatistics(int? id)
        {
            if(id == null || id == 0)
                return NotFound();

            var obj = _context.Footballers.Include(f => f.Statistics).FirstOrDefault(x => x.Id == id);

            if(obj == null)
                return NotFound();

            return View(obj);
        }

        [HttpPost]
        [Authorize(Policy = "AdminAccess")]
        //[Authorize(Policy = "CoachAccess")]
        public IActionResult EditStatistics([FromForm]Statistics statistics)
        {
            var editedData = _context.Statistics.FirstOrDefault(s => s.Id == statistics.Id);
            if (editedData == null)
                return NotFound(statistics);

            // Warunek aby sprawdzić czy po odjęciu wartośc byłaby mniejsza niż zero
            // Jest tutaj + w drugim warunku bo przekazana wartość to -40 więc przy - by dodało 
            if(statistics.Minutes < 0 && editedData.Minutes + statistics.Minutes < 0)
            {
                TempData["Alert"] = $"Odjęta wartośc byłaby mniejsza niż zero | Aktualnie {editedData.Minutes} minut";
                return RedirectToAction("EditStatistics", new { id = editedData.Id });
            }
            else
            {
                editedData.Minutes += statistics.Minutes;
                int fullMatches = editedData.Minutes / 90;
                editedData.Match = fullMatches;
            }

            if(statistics.Goals < 0 && editedData.Goals + statistics.Goals < 0)
            {
                TempData["Alert"] = $"Odjęta wartośc byłaby mniejsza niż zero | Aktualnie {editedData.Goals} goli";
                return RedirectToAction("EditStatistics", new { id = editedData.Id });
            }
            else
                editedData.Goals += statistics.Goals;

            if (statistics.Assists < 0 && editedData.Assists + statistics.Assists < 0)
            {
                TempData["Alert"] = $"Odjęta wartośc byłaby mniejsza niż zero | Aktualnie {editedData.Assists} asyst";
                return RedirectToAction("EditStatistics", new { id = editedData.Id });
            }
            else
                editedData.Assists += statistics.Assists;

            if (statistics.YellowCards < 0 && editedData.YellowCards + statistics.YellowCards < 0)
            {
                TempData["Alert"] = $"Odjęta wartośc byłaby mniejsza niż zero | Aktualnie {editedData.YellowCards} żółtych kartek";
                return RedirectToAction("EditStatistics", new { id = editedData.Id });
            }
            else
                editedData.YellowCards += statistics.YellowCards;

            if (statistics.RedCards < 0 && editedData.RedCards + statistics.RedCards < 0)
            {
                TempData["Alert"] = $"Odjęta wartośc byłaby mniejsza niż zero | Aktualnie {editedData.RedCards} minut";
                return RedirectToAction("EditStatistics", new { id = editedData.Id });
            }
            else
                editedData.RedCards += statistics.RedCards;


            //_context.Statistics.Update(statistics);
            _context.SaveChanges();
            return RedirectToAction("ShowStatistics");
        }
    }
}
