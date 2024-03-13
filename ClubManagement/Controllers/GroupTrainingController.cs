using ClubManagement.ApplicationDbContext;
using ClubManagement.Models;
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
        public IActionResult ShowGT()
        {
            IEnumerable<GroupTraining> gt = _context.GroupTraining;

            return View(gt);
        }

        // Dodawanie
        [HttpGet]
        public IActionResult PrepareToAddGT()
        {
            ViewBag.Footballers = _context.Footballers.ToList();

            return View();
        }

        [HttpPost]
        public IActionResult AddGT([FromForm]GroupTraining gt, [FromForm]List<int> selectedPlayers)
        {
            // Sprawdzenie czy podane data i czas nie są wcześniejsze niż chwila obecna
            DateTime now = DateTime.Now;

            if (gt.DateOfTraining.Date < now.Date ||
                (gt.DateOfTraining.Date == now.Date && gt.StartTraining.TimeOfDay < now.TimeOfDay))
            {
                TempData["Alert"] = "Data lub godzina rozpoczęcia jest wcześniejsza niż obecna chwila";
                return RedirectToAction("PrepareToAddGT");
            }

            gt.StartTraining = gt.DateOfTraining.Date + gt.StartTraining.TimeOfDay;
            gt.EndTraining = gt.DateOfTraining.Date + gt.EndTraining.TimeOfDay;

            if (gt.EndTraining < gt.StartTraining)
            {
                TempData["Alert"] = "Zakończenie treningu jest wcześniej niż rozpoczęcie";
                return RedirectToAction("PrepareToAddGT");
            }

            gt.TimeOfTraining = gt.EndTraining - gt.StartTraining;

            _context.GroupTraining.Add(gt);

            foreach (var footballerId in selectedPlayers)
            {
                var footballer = _context.Footballers.Find(footballerId);
                if (footballer != null)
                {
                    gt.Footballers.Add(footballer);
                }
            }

            _context.SaveChanges();
            TempData["Success"] = "Pomyslnie dodano trening";

            return RedirectToAction("ShowGT");
        }

        // Edytowanie
        [HttpGet]
        public IActionResult PrepareToEditGT(int? id)
        {
            if (id == null || id == 0)
                return NotFound("PrepareToEditGT - nie ma id ");

            var obj = _context.GroupTraining.Include(gt => gt.Footballers).FirstOrDefault(x => x.Id == id);

            if (obj == null)
                return NotFound("PrepareToEditGT - nie ma obj w bazie danych");

            ViewBag.Footballers = _context.Footballers.ToList();
            ViewBag.SelectedPlayers = obj.Footballers.Select(f => f.Id).ToList();

            return View(obj);
        }

        [HttpPost]
        public IActionResult EditGT([FromForm]GroupTraining gt, [FromForm] List<int> selectedPlayers)
        { 
            DateTime now = DateTime.Now;

            if (gt.DateOfTraining.Date < now.Date ||
                (gt.DateOfTraining.Date == now.Date && gt.StartTraining.TimeOfDay < now.TimeOfDay))
            {
                TempData["Alert"] = "Data lub godzina rozpoczęcia jest wcześniejsza niż obecna chwila";
                return RedirectToAction("PrepareToEditGT");
            }

            gt.StartTraining = gt.DateOfTraining.Date + gt.StartTraining.TimeOfDay;
            gt.EndTraining = gt.DateOfTraining.Date + gt.EndTraining.TimeOfDay;

            if (gt.EndTraining < gt.StartTraining)
            {
                TempData["Alert"] = "Zakończenie treningu jest wcześniej niż rozpoczęcie";
                return RedirectToAction("PrepareToEditGT");
            }

            gt.TimeOfTraining = gt.EndTraining - gt.StartTraining;

            _context.GroupTraining.Update(gt);

            var existingGT = _context.GroupTraining
                .Include(gt => gt.Footballers)
                .FirstOrDefault(x => x.Id == gt.Id);

            if (existingGT == null)
                return NotFound("EditGT - Nie znaleziono obiektu");

            // Usuń istniejące powiązania z piłkarzami
            existingGT.Footballers.Clear();

            // Dodaj nowe powiązania do piłkarzy z listy selectedPlayers
            foreach (var footballerId in selectedPlayers)
            {
                var footballer = _context.Footballers.Find(footballerId);
                if (footballer != null)
                {
                    existingGT.Footballers.Add(footballer);
                }
            }

            _context.SaveChanges();
            TempData["Success"] = "Pomyslnie zedytowano trening trening";

            return RedirectToAction("ShowGT");
        }

        // Usuń
        [HttpGet]
        public IActionResult RemoveGT(int? id)
        {
            if(id == null || id == 0)
                return NotFound("RemoveGT - nie ma id");

            var obj = _context.GroupTraining.FirstOrDefault(gt => gt.Id == id);

            if (obj == null) 
                return NotFound("RemoveGT - Nie znaleziono obiektu");

            _context.GroupTraining.Remove(obj);
            _context.SaveChanges();

            return RedirectToAction("ShowGT");
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
