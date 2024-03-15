using ClubManagement.ApplicationDbContext;
using ClubManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ClubManagement.Controllers
{
    public class PlayerController : Controller
    {
        private readonly ClubDbContext _context;

        public PlayerController(ClubDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            IEnumerable<Footballer> footballers = _context.Footballers
                .Include(f => f.Statistics)
                .ToList();

            return View(footballers);
        }

        // Add Footballer
        [HttpGet]
        [Authorize(Policy = "AdminAccess")]
        public IActionResult AddPlayer(int? accountId)
        {
            if (accountId == null)
                return RedirectToAction("register", "Account");
            else
                ViewBag.AccountId = accountId;
                return View();
        }

        [HttpPost]
        [Authorize(Policy = "AdminAccess")]
        public IActionResult AddPlayer([FromForm]Footballer footballer)
        {
            // walidacja do roku urodzenia oraz kategorii wiekowej
            DateTime now = DateTime.Now;
            int playerAge = now.Year - footballer.DateOfBirth.Value.Year;
            DateTime givenData = footballer.DateOfBirth.Value.AddYears(playerAge);
            if ( givenData < now )
                playerAge--;// potrzebne aby mieć rzeczywisty wiek a nie kalendarzowy

            switch (footballer.AgeCategory)
            {
                case "firstTeam":
                case "reserves":
                    if(playerAge < 18)
                    {
                        TempData["Alert"] = "Do pierwszej drużyny i rezerw zawodnik musi mieć minimum 18 lat";
                        return RedirectToAction("AddPlayer", new { accountId = footballer.AccountId });
                    }
                    break;
                case "juniors":
                    if(playerAge < 13 || playerAge >= 18)
                    {
                        TempData["Alert"] = "Do drużyny juniorów zawodnik nie może mieć mniej 13 i więcej niż 18 lat";
                        return RedirectToAction("AddPlayer", new { accountId = footballer.AccountId });
                    }
                    break;
                case "sneakers":
                    if (playerAge < 10 || playerAge >= 15)
                    {
                        TempData["Alert"] = "Do drużyny trampkarzy zawodnik nie może mieć mniej 10 i więcej niż 15 lat";
                        return RedirectToAction("AddPlayer", new { accountId = footballer.AccountId });
                    }
                    break;
                default:
                    TempData["Alert"] = "Podany wiek zawodnika nie mieści się w żadną kategorię";
                    return RedirectToAction("AddPlayer", new { accountId = footballer.AccountId });
            }

            // walidacja do numeru gracza
            var sameNumber = _context.Footballers.Any(f => f.PlayerNumber == footballer.PlayerNumber);
            if (sameNumber)
            {
                TempData["Alert"] = "Ktoś już posiada taki numer";
                return RedirectToAction("AddPlayer", new { accountId = footballer.AccountId });
            }
            else
            {
                // Domyślnie inicjuje zerami
                var statistics = new Statistics();
                footballer.Statistics = statistics;

                _context.Footballers.Add(footballer);
                _context.SaveChanges();

                // Id do account
                var accountId = _context.Accounts.FirstOrDefault(a => a.Id == footballer.AccountId);
                if (accountId == null)
                    return NotFound();

                accountId.FootballerId = footballer.Id;
                _context.Accounts.Update(accountId);
                _context.SaveChanges();

                return RedirectToAction("Index", "Player");
            }
        }

        // Edit Footballer
        [HttpGet]
        [Authorize(Policy = "AdminAccess")]
        public IActionResult EditPlayer(int? id) 
        {
            if (id == null || id == 0)
                return NotFound();

            var obj = _context.Footballers.FirstOrDefault(f => f.Id == id);

            if (obj == null)
                return NotFound();

            return View(obj);
        }

        [HttpPost]
        [Authorize(Policy = "AdminAccess")]
        public IActionResult EditPlayer([FromForm]Footballer footballer)
        {
            var footballerToEdit = _context.Footballers.FirstOrDefault(x => x.Id == footballer.Id);
            if (footballerToEdit == null)
                return NotFound();

            // Jeśli ktoś posiada już taki numer oraz jest to inna osoba niż edytowany zawodnik
            if (_context.Footballers.Any(f => f.PlayerNumber == footballer.PlayerNumber && f.Id != footballer.Id))
            {
                TempData["Alert"] = "Numer zawodnika jest już zajęty";
                return RedirectToAction("EditPlayer", new { id = footballer.Id });
            }

            // Na razie tak błędy komplikuje instancja footballerToedit
            footballerToEdit.Name = footballer.Name;
            footballerToEdit.LastName = footballer.LastName;
            footballerToEdit.Country = footballer.Country;
            footballerToEdit.DateOfBirth = footballer.DateOfBirth;
            footballerToEdit.Growth = footballer.Growth;
            footballerToEdit.Weight = footballer.Weight;
            footballerToEdit.Position = footballer.Position;
            footballerToEdit.PlayerNumber = footballer.PlayerNumber;
            footballerToEdit.WhichFoot = footballer.WhichFoot;

            //_context.Footballers.Update(footballerToEdit);
            _context.SaveChanges();
            TempData["Success"] = "Pomyślnie zaktualizowano gracza";
            return RedirectToAction("Index");
        }

        // Remove Player
        [HttpGet]
        [Authorize(Policy = "AdminAccess")]
        public IActionResult RemovePlayer(int id)
        {
            var footballerToRemove = _context.Footballers.Include(f => f.Statistics).FirstOrDefault(f => f.Id == id);
            if (footballerToRemove != null)
                _context.Footballers.Remove(footballerToRemove);
            else
                return RedirectToAction("Index");

            var accountToRemove = _context.Accounts.FirstOrDefault(a => a.Id == footballerToRemove.AccountId);
            if (accountToRemove != null)
                _context.Accounts.Remove(accountToRemove);

            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
