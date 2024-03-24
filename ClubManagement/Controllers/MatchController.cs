using ClubManagement.ApplicationDbContext;
using ClubManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ClubManagement.Controllers
{
    public class MatchController : Controller
    {
        private readonly ClubDbContext _context;
        public MatchController(ClubDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        //[Authorize(Policy = "")]
        public IActionResult ShowMatches(string? filterCategory = "firstTeam")
        {
            ViewBag.FilterCategory = filterCategory;

            IQueryable<Match> m = _context.Matches
                   .Where(m => m.AgeCategory == filterCategory)
                   .OrderBy(m => m.DateOfMatch);

            return View(m);
        }

        [HttpGet]
        //[Authorize(Policy = "")]
        public IActionResult MatchSchedule()
        {
            var currentTime = DateTime.Now;
            var match = _context.Matches.Where(m => m.StartMatch > currentTime).ToList();

            return View(match);
        }

        [HttpGet]
        //[Authorize(Policy = "")]
        public IActionResult MatchResults()
        {
            var currentTime = DateTime.Now;
            var obj = _context.Matches.Where(m => m.StartMatch < currentTime).ToList();

            return View(obj);
        }

        [HttpGet]
        //[Authorize(Policy = "")]
        public IActionResult MatchDetails(int? id)
        {
            if (id == null)
                return NotFound();

            var match = _context.Matches
                .Include(m => m.MainCoach) 
                .Include(m => m.PrimaryMatchPlayers) 
                .Include(m => m.SubstituteMatchPlayers) 
                .FirstOrDefault(m => m.Id == id);

            if (match == null)
                return NotFound();

            // Przekazanie informacji o zawodnikach do ViewBag
            ViewBag.PrimaryPlayers = _context.Footballers
                .Where(p => match.PrimaryMatchPlayers.Select(pm => pm.IdForEleven).Contains(p.Id))
                .ToList();
            ViewBag.SubstitutePlayers = _context.Footballers
                .Where(p => match.SubstituteMatchPlayers.Select(sm => sm.IdForSubstitute).Contains(p.Id))
                .ToList();


            return View(match);
        }

        [HttpGet]
        public IActionResult LastMatchesWithOpponent(string? opponentName)
        {
            if(opponentName == null || opponentName == "")
                return NotFound();

            // Pobierz ostatnie mecze z danym przeciwnikiem
            var lastMatches = _context.Matches
                .Where(m => m.Enemy == opponentName)
                .OrderByDescending(m => m.DateOfMatch)
                .Take(10) // Wybierz 10 ostatnich meczów
                .ToList();

            ViewBag.OpponentName = opponentName;

            return View(lastMatches);
        }

        /////////////////////
        [HttpGet]
        //[Authorize(Policy = "")]
        public IActionResult PrepareToAddMatch(string? filterCategory = "firstTeam")
        {
            ViewBag.FilterCategory = filterCategory;
            return View();
        }

        [HttpPost]
        //[Authorize(Policy = "")]
        public IActionResult AddMatch([FromForm]Match m, [FromForm] List<int> selectedPlayers)
        {
            // Sprawdzenie czy podane data i czas nie są wcześniejsze niż chwila obecna
            if(m.DateOfMatch != null)
            {
                DateTime now = DateTime.Now;

                if (m.DateOfMatch.Value.Date < now.Date ||
                    (m.DateOfMatch.Value.Date == now.Date && m.StartMatch.Value.TimeOfDay < now.TimeOfDay))
                {
                    TempData["Alert"] = "Data lub godzina rozpoczęcia jest wcześniejsza niż obecna chwila";
                    return RedirectToAction("PrepareToAddMatch", new { filterCategory = m.AgeCategory });
                }
            }


            _context.Matches.Add(m);

            _context.SaveChanges();
            TempData["Success"] = "Pomyslnie dodano trening";

            return RedirectToAction("ShowMatches", new { filterCategory = m.AgeCategory });
        }

        [HttpGet]
        //[Authorize(Policy = "")]
        public IActionResult PrepareToEditMatch(int? id)
        {
            if (id == null || id == 0)
                return NotFound("PrepareToEditMatch - nie ma id ");

            var obj = _context.Matches.FirstOrDefault(x => x.Id == id);

            if (obj == null)
                return NotFound("PrepareToEditMatch - nie ma obj w bazie danych");

            return View(obj);
        }

        [HttpPost]
        //[Authorize(Policy = "")]
        public IActionResult EditMatch([FromForm] Match m, int? TeamGoals, int? OpponentGoals)
        {
            // Sprawdzenie czy podane data i czas nie są wcześniejsze niż chwila obecna
            if (m.DateOfMatch != null)
            {
                DateTime now = DateTime.Now;

                if ((m.MatchStatus == "coming" || m.MatchStatus == "canceled" || m.MatchStatus == "delayed") &&
                    (m.DateOfMatch.Value.Date < now.Date ||
                    (m.DateOfMatch.Value.Date == now.Date && (m.StartMatch != null && m.StartMatch.Value.TimeOfDay < now.TimeOfDay))))
                {
                    TempData["Alert"] = "Jeśli mecz jest nadchodzący lub przełożony, data lub godzina rozpoczęcia musi być wcześniejsza niż obecna chwila";
                    return RedirectToAction("PrepareToEditMatch", new { id = m.Id });
                }

                if (m.MatchStatus == "finished" &&
                    (m.DateOfMatch.Value.Date > now.Date ||
                    (m.DateOfMatch.Value.Date == now.Date && (m.StartMatch != null && m.StartMatch.Value.TimeOfDay > now.TimeOfDay))))
                {
                    TempData["Alert"] = "Jeśli mecz jest skończony, data lub godzina rozpoczęcia musi być wcześniejsza niż obecna chwila";
                    return RedirectToAction("PrepareToEditMatch", new { id = m.Id });
                }
            }

            // walidacja do wyniku
            if (TeamGoals != null && OpponentGoals != null)
            {
                if (TeamGoals < 0 || OpponentGoals < 0)
                {
                    TempData["Alert"] = "Żle podałeś wyniki drużyn nie mogą być mniejsze niż 0";
                    return RedirectToAction("PrepareToEditMatch", new { id = m.Id });
                }

                string score;
                string scoreStatus;

                if (TeamGoals > OpponentGoals)
                    scoreStatus = "win";
                else if (TeamGoals < OpponentGoals)
                    scoreStatus = "loss";
                else
                    scoreStatus = "draw";

                if (m.MatchHost == "home")
                    score = $"{TeamGoals} - {OpponentGoals}";
                else
                    score = $"{OpponentGoals} - {TeamGoals}";

                m.Score = score;
                m.ScoreStatus = scoreStatus;
            }


            _context.Matches.Update(m);

            _context.SaveChanges();
            TempData["Success"] = "Pomyslnie zedytowano trening trening";

            return RedirectToAction("ShowMatches", new { filterCategory = m.AgeCategory });
        }

        [HttpGet]
        public IActionResult RemoveMatch(int? id)
        {
            if (id == null || id == 0)
                return NotFound("RemoveMatch - nie ma id");

            var obj = _context.Matches.FirstOrDefault(gt => gt.Id == id);

            if (obj == null)
                return NotFound("RemoveGT - Nie znaleziono obiektu");

            _context.Matches.Remove(obj);
            _context.SaveChanges();

            return RedirectToAction("ShowMatches", new { filterCategory = obj.AgeCategory });
        }

        [HttpGet]
        //[Authorize(Policy = "")]
        public IActionResult PrepareToAddTrainer(int? id)
        {
            if (id == null || id == 0)
                return NotFound("PrepareToAddStaff - brak id");

            var obj = _context.Matches.FirstOrDefault(m => m.Id == id);

            if (obj == null)
                return NotFound("PrepareToAddTrainer - brak obj w bazie danych");

            ViewBag.Coaches = _context.Coaches;

            return View(obj);
        }

        [HttpPost]
        //[Authorize(Policy = "")]
        public IActionResult AddTrainer([FromForm]Match match)
        {
            var obj = _context.Matches.FirstOrDefault(m => m.Id == match.Id);
            if (obj == null)
                return NotFound("AddTrainer - nie ma w bazie");

            obj.MainCoachId = match.MainCoachId;
            
            _context.SaveChanges();

            return RedirectToAction("ShowMatches", new { filterCategory = obj.AgeCategory });
        }

        [HttpGet]
        //[Authorize(Policy = "")]
        public IActionResult PrepareToAddStaff(int? id)
        {
            if (id == null || id == 0)
                return NotFound("Brak ID meczu.");

            var match = _context.Matches.FirstOrDefault(m => m.Id == id);

            if (match == null)
                return NotFound("SAdas");

            var primaryPlayers = _context.PrimaryMatchPlayers
                                .Where(p => p.MatchId == match.Id)
                                .Select(p => p.IdForEleven)
                                .ToList();

            // Pobranie piłkarzy należących do rezerw
            var substitutePlayers = _context.SubstituteMatchPlayers
                                            .Where(s => s.MatchId == match.Id)
                                            .Select(s => s.IdForSubstitute)
                                            .ToList();


            ViewBag.Players = _context.Footballers.Where(f => f.AgeCategory == match.AgeCategory);
            ViewBag.PrimaryPlayers = primaryPlayers;
            ViewBag.SubstitutePlayers = substitutePlayers;

            return View(match);
        }

        [HttpPost]
        //[Authorize(Policy = "")]
        public IActionResult AddStaff([FromForm] Match m, [FromForm] List<int> selectedElevenPlayers, [FromForm] List<int> selectedSubstitutePlayers)
        {
            var match = _context.Matches.Include(m => m.Footballers)
                                 .Include(m => m.PrimaryMatchPlayers)
                                 .Include(m => m.SubstituteMatchPlayers)
                                 .FirstOrDefault(match => match.Id == m.Id);

            if (match == null)
                return NotFound("Mecz nie istnieje");

            // Sprawdzenie, czy liczba wybranych zawodników w podstawowej jedenastce mieści się w zakresie 7-11
            if (selectedElevenPlayers.Count < 7 || selectedElevenPlayers.Count > 11)
            {
                TempData["Alert"] = "Podstawowa skład musi zawierać od 7 do 11 zawodników.";
                return RedirectToAction("PrepareToAddStaff", new { id = match.Id });
            }

            // Wyczyszczenie kolekcji graczy w meczu
            match.Footballers.Clear();
            match.PrimaryMatchPlayers.Clear();
            match.SubstituteMatchPlayers.Clear();

            // Dodanie graczy do podstawowej jedenastki
            foreach (var playerId in selectedElevenPlayers)
            {
                var footballer = _context.Footballers.Find(playerId);
                if (footballer != null)
                {
                    match.Footballers.Add(footballer);
                    match.PrimaryMatchPlayers.Add(new PrimaryMatchPlayer { IdForEleven = playerId });
                }
            }

            // Dodanie graczy do rezerw
            foreach (var playerId in selectedSubstitutePlayers)
            {
                var footballer = _context.Footballers.Find(playerId);
                if (footballer != null)
                {
                    match.Footballers.Add(footballer);
                    match.SubstituteMatchPlayers.Add(new SubstituteMatchPlayer { IdForSubstitute = playerId });
                }
            }

            _context.Matches.Update(match);
            _context.SaveChanges();

            return RedirectToAction("ShowMatches", new { filterCategory = match.AgeCategory });
        }

        [HttpGet]
        //[Authorize(Policy = "")]
        public IActionResult ClearMatchPlayers(int? id)
        {
            if(id == null || id == 0)
                return NotFound("ClearMatchPlayers - brak id");

            var match = _context.Matches
                                .Include(m => m.Footballers)
                                .Include(m => m.PrimaryMatchPlayers)
                                .Include(m => m.SubstituteMatchPlayers)
                                .FirstOrDefault(m => m.Id == id);

            if (match == null)
                return NotFound("Mecz nie istnieje");

            // Wyczyszczenie kolekcji graczy w meczu
            match.Footballers.Clear();
            match.PrimaryMatchPlayers.Clear();
            match.SubstituteMatchPlayers.Clear();

            _context.SaveChanges();

            return RedirectToAction("ShowMatches", new { filterCategory = match.AgeCategory });
        }

    }
}
