using ClubManagement.ApplicationDbContext;
using ClubManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ClubManagement.Controllers
{
    public class IndividualTrainingController : Controller
    {
        private readonly ClubDbContext _context;
        public IndividualTrainingController(ClubDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult ShowPlayers()
        {
            IEnumerable<Footballer> footballers = _context.Footballers;
            return View(footballers);
        }

        [HttpGet]
        public IActionResult ShowIT(int? id)
        {
            if(id == null || id == 0)
                return NotFound();

            var obj = _context.Footballers.Include(f => f.IndividualTrainings)
                .ThenInclude(it => it.Coach) // Włącz dane o trenerach
                .FirstOrDefault(x => x.Id == id);

            if(obj == null)
                return NotFound();

            return View(obj);
        }

        // Dodawanie
        [HttpGet]
        public IActionResult PrepareToAddIT(int? id)
        {
            if(id == null || id == 0)
                return NotFound();

            IQueryable<Coach> coaches = _context.Coaches;           

            ViewBag.FootballerId = id;
            ViewBag.Coaches = coaches;

            return View();
        }

        [HttpPost]
        public IActionResult AddIT([FromForm]IndividualTraining it)
        {
            // Sprawdzenie czy podane data i czas nie są wcześniejsze niż chwila obecna
            DateTime now = DateTime.Now;

            if (it.DateOfTraining.Date < now.Date ||
                (it.DateOfTraining.Date == now.Date && it.StartTraining.TimeOfDay < now.TimeOfDay))
            {
                TempData["Alert"] = "Data lub godzina rozpoczęcia jest wcześniejsza niż obecna chwila";
                return RedirectToAction("PrepareToAddIT", new { id = it.FootballerId });
            }

            it.StartTraining = it.DateOfTraining.Date + it.StartTraining.TimeOfDay;
            it.EndTraining = it.DateOfTraining.Date + it.EndTraining.TimeOfDay;

            if (it.EndTraining < it.StartTraining)
            {
                TempData["Alert"] = "Zakończenie treningu jest wcześniej niż rozpoczęcie";
                return RedirectToAction("PrepareToAddIT", new { id = it.FootballerId });
            }

            it.TimeOfTraining = it.EndTraining - it.StartTraining;

            _context.IndividualTraining.Add(it);
            _context.SaveChanges();

            TempData["Success"] = "Pomyslnie dodano trening";
            return RedirectToAction("ShowIT", new { id = it.FootballerId });
        }

        // Edytowanie
        public IActionResult PrepareToEditIT(int? id)
        {
            if(id  == null || id == 0)
                return NotFound();

            var obj = _context.IndividualTraining.FirstOrDefault(it => it.Id == id);

            if (obj == null) 
                return NotFound();

            ViewBag.Coaches = _context.Coaches;

            return View(obj);
        }

        public IActionResult EditIT([FromForm]IndividualTraining it) 
        {
            // Sprawdzenie czy podane data i czas nie są wcześniejsze niż chwila obecna
            DateTime now = DateTime.Now;

            if (it.DateOfTraining.Date < now.Date ||
                (it.DateOfTraining.Date == now.Date && it.StartTraining.TimeOfDay < now.TimeOfDay))
            {
                TempData["Alert"] = "Data,godzina rozpoczęcia lub zakończenia treningu jest wcześniejsza niż obecna chwila";
                return RedirectToAction("PrepareToEditIT", new { id = it.Id });
            }

            it.StartTraining = it.DateOfTraining.Date + it.StartTraining.TimeOfDay;
            it.EndTraining = it.DateOfTraining.Date + it.EndTraining.TimeOfDay;

            if (it.EndTraining < it.StartTraining)
            {
                TempData["Alert"] = "Zakończenie treningu jest wcześniej niż rozpoczęcie";
                return RedirectToAction("PrepareToEditIT", new { id = it.Id });
            }

            it.TimeOfTraining = it.EndTraining - it.StartTraining;

            _context.IndividualTraining.Update(it);
            _context.SaveChanges();

            return RedirectToAction("ShowIT", new { id = it.FootballerId});
        }
        // Usuwanie
        public IActionResult RemoveIT(int? id)
        {
            if(id == null || id == 0)
                return NotFound();

            var obj = _context.IndividualTraining.FirstOrDefault(x => x.Id == id);

            if (obj == null)
                return NotFound();

            int footballerId = obj.FootballerId;

            _context.IndividualTraining.Remove(obj);
            _context.SaveChanges();

            return RedirectToAction("ShowIT", new { id = footballerId });
        }

        public IActionResult RemoveAllIT(int? id)
        {
            if (id == null || id == 0)
                return NotFound();

            // Znajdź wszystkie treningi dla danego FootballerId
            var trainingsToRemove = _context.IndividualTraining.Where(x => x.FootballerId == id).ToList();

            if (trainingsToRemove == null || trainingsToRemove.Count == 0)
                return NotFound();

            int footballerId = trainingsToRemove.First().FootballerId;

            // Usuń wszystkie znalezione treningi
            _context.IndividualTraining.RemoveRange(trainingsToRemove);
            _context.SaveChanges();

            return RedirectToAction("ShowIT", new { id = footballerId });
        }
    }
}
