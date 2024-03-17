using ClubManagement.ApplicationDbContext;
using ClubManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClubManagement.Controllers
{
    public class GroupTrainingController : Controller
    {
        private readonly ClubDbContext _context;

        public GroupTrainingController(ClubDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult ShowGT(string? filterCategory = "firstTeam")
        {
            ViewBag.FilterCategory = filterCategory;

            IQueryable<GroupTraining> gt = _context.GroupTraining
                   .Include(gt => gt.Coaches)
                   .Where(gt => gt.AgeCategory == filterCategory)
                   .OrderBy(gt => gt.DateOfTraining);

            return View(gt);
        }

        // Dodawanie
        [HttpGet]
        //[Authorize(Policy = "")]
        public IActionResult PrepareToAddGT(string? filterCategory)
        {
            if (filterCategory == null)
                return NotFound("PrepareToAddGT - brak przekazanej kategorii");

            //IEnumerable<Footballer> players;
            IQueryable<Footballer> players = _context.Footballers.Where(f => f.AgeCategory == filterCategory);
            IQueryable<Coach> coaches = _context.Coaches;

            ViewBag.FilterCategory = filterCategory;
            ViewBag.Footballers = players;
            ViewBag.Coaches = coaches;

            return View();
        }

        [HttpPost]
        public IActionResult AddGT([FromForm]GroupTraining gt, [FromForm]List<int> selectedPlayers, [FromForm]List<int> selectedCoaches, [FromForm]string filterCategory)
        {
            // Sprawdzenie czy podane data i czas nie są wcześniejsze niż chwila obecna
            DateTime now = DateTime.Now;

            if (gt.DateOfTraining.Date < now.Date ||
                (gt.DateOfTraining.Date == now.Date && gt.StartTraining.TimeOfDay < now.TimeOfDay))
            {
                TempData["Alert"] = "Data lub godzina rozpoczęcia jest wcześniejsza niż obecna chwila";
                return RedirectToAction("PrepareToAddGT", new { filterCategory = filterCategory });
            }

            gt.StartTraining = gt.DateOfTraining.Date + gt.StartTraining.TimeOfDay;
            gt.EndTraining = gt.DateOfTraining.Date + gt.EndTraining.TimeOfDay;

            if (gt.EndTraining < gt.StartTraining)
            {
                TempData["Alert"] = "Zakończenie treningu jest wcześniej niż rozpoczęcie";
                return RedirectToAction("PrepareToAddGT", new { filterCategory = filterCategory });
            }

            gt.TimeOfTraining = gt.EndTraining - gt.StartTraining;

            _context.GroupTraining.Add(gt);

            // Dodawanie graczy
            foreach (var footballerId in selectedPlayers)
            {
                var footballer = _context.Footballers.Find(footballerId);
                if (footballer != null)
                    gt.Footballers.Add(footballer);
            }
            // Dodawanie trenerów
            foreach (var coachId in selectedCoaches)
            {
                var coach = _context.Coaches.Find(coachId);
                if (coach != null)
                    gt.Coaches.Add(coach);
            }

            _context.SaveChanges();
            TempData["Success"] = "Pomyslnie dodano trening";

            return RedirectToAction("ShowGT", new { filterCategory = filterCategory });
        }

        // Edytowanie
        [HttpGet]
        public IActionResult PrepareToEditGT(int? id, string? filterCategory)
        {
            if (id == null || id == 0)
                return NotFound("PrepareToEditGT - nie ma id ");
            if (filterCategory == null)
                return NotFound("PrepareToEditGT - nie ma filterCategory");

            var obj = _context.GroupTraining
                .Include(gt => gt.Footballers)
                .Include(gt => gt.Coaches)
                .FirstOrDefault(x => x.Id == id);

            // do players
            IQueryable<Footballer> players = _context.Footballers.Where(f => f.AgeCategory == filterCategory);

            if (obj == null)
                return NotFound("PrepareToEditGT - nie ma obj w bazie danych");

            ViewBag.FilterCategory = filterCategory;
            ViewBag.Footballers = players;
            ViewBag.SelectedPlayers = obj.Footballers.Select(f => f.Id).ToList();
            ViewBag.Coaches = _context.Coaches.ToList();
            ViewBag.SelectedCoaches = obj.Coaches.Select(f => f.Id).ToList();

            return View(obj);
        }

        [HttpPost]
        public IActionResult EditGT([FromForm]GroupTraining gt, [FromForm] List<int> selectedPlayers, [FromForm] List<int> selectedCoaches, [FromForm] string filterCategory)
        { 
            DateTime now = DateTime.Now;

            if (gt.DateOfTraining.Date < now.Date ||
                (gt.DateOfTraining.Date == now.Date && gt.StartTraining.TimeOfDay < now.TimeOfDay))
            {
                TempData["Alert"] = "Data lub godzina rozpoczęcia jest wcześniejsza niż obecna chwila";
                return RedirectToAction("PrepareToEditGT", new { id = gt.Id, filterCategory = filterCategory });
            }

            gt.StartTraining = gt.DateOfTraining.Date + gt.StartTraining.TimeOfDay;
            gt.EndTraining = gt.DateOfTraining.Date + gt.EndTraining.TimeOfDay;

            if (gt.EndTraining < gt.StartTraining)
            {
                TempData["Alert"] = "Zakończenie treningu jest wcześniej niż rozpoczęcie";
                return RedirectToAction("PrepareToEditGT", new { id = gt.Id, filterCategory = filterCategory });
            }

            gt.TimeOfTraining = gt.EndTraining - gt.StartTraining;

            _context.GroupTraining.Update(gt);

            var existingGT = _context.GroupTraining
                .Include(gt => gt.Footballers)
                .Include(gt => gt.Coaches)
                .FirstOrDefault(x => x.Id == gt.Id);

            if (existingGT == null)
                return NotFound("EditGT - Nie znaleziono obiektu existingGT");

            // Usuń istniejące powiązania z piłkarzami
            existingGT.Footballers.Clear();
            // Dodaj nowe powiązania do piłkarzy z listy selectedPlayers
            foreach (var footballerId in selectedPlayers)
            {
                var footballer = _context.Footballers.Find(footballerId);
                if (footballer != null)
                    existingGT.Footballers.Add(footballer);
            }

            // Usuń istniejące powiązania z trenerami
            existingGT.Coaches.Clear();
            // Dodaj nowe powiązania do trenerów z listy selectedCoaches
            foreach (var coachId in selectedCoaches)
            {
                var coach = _context.Coaches.Find(coachId);
                if (coach != null)
                    existingGT.Coaches.Add(coach);
            }

            _context.SaveChanges();
            TempData["Success"] = "Pomyslnie zedytowano trening trening";

            return RedirectToAction("ShowGT", new { filterCategory = filterCategory });
        }

        // Usuń
        [HttpGet]
        public IActionResult RemoveGT(int? id, string filterCategory)
        {
            if(id == null || id == 0)
                return NotFound("RemoveGT - nie ma id");

            var obj = _context.GroupTraining.FirstOrDefault(gt => gt.Id == id);

            if (obj == null) 
                return NotFound("RemoveGT - Nie znaleziono obiektu");

            _context.GroupTraining.Remove(obj);
            _context.SaveChanges();

            return RedirectToAction("ShowGT", new { filterCategory = filterCategory });
        }

        [HttpGet]
        public IActionResult RemoveAllGT()
        {
            var trainingsToRemove = _context.GroupTraining.ToList();

            if (trainingsToRemove == null || trainingsToRemove.Count == 0)
                return NotFound();

            _context.GroupTraining.RemoveRange(trainingsToRemove);
            _context.SaveChanges();

            return RedirectToAction("ShowGT");
        }
    }
}
