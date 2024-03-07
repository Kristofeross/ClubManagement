using ClubManagement.ApplicationDbContext;
using ClubManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult AddPlayer(int accountId)
        {
            if (accountId > 0)
            {
                ViewBag.AccountId = accountId;
                return View();
            }
            else
            {
                return RedirectToAction("register", "Account");
            }
        }

        [HttpPost]
        [Authorize(Policy = "AdminAccess")]
        public IActionResult AddPlayer([FromForm]Footballer footballer)
        {
            var sameNumber = _context.Footballers.Any(f => f.PlayerNumber == footballer.PlayerNumber);
            if (sameNumber)
            {
                TempData["Alert"] = "Ktoś już posiada taki numer";
                return RedirectToAction("AddPlayer", new { accountId = footballer.AccountId });
            }
            else
            {
                // Domyślnie inicjuje zerami
                /*var statistics = new Statistics
                {
                    Match = 0,
                    Minutes = 0,
                    Goals = 0,
                    Assists = 0,
                    YellowCards = 0,
                    RedCards = 0,
                };*/

                var statistics = new Statistics();
                footballer.Statistics = statistics;

                _context.Footballers.Add(footballer);
                _context.SaveChanges();

                // Id do stytystyk
                statistics.FootballerId = footballer.Id;
                // Id do account
                var accountId = _context.Accounts.FirstOrDefault(a => a.Id == footballer.AccountId);
                if (accountId == null)
                    return NotFound();

                accountId.FootballerId = footballer.Id;
                _context.Accounts.Update(accountId);
                _context.Statistics.Update(statistics);
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
            _context.Footballers.Update(footballer);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        // Remove Player
        [HttpGet]
        [Authorize(Policy = "AdminAccess")]
        public IActionResult RemovePlayer(int id)
        {
            // Wersja w chuj niezoptymalizowana
            var footballerToRemove = _context.Footballers.FirstOrDefault(f => f.Id == id);
            if (footballerToRemove == null)
                return NotFound();
            var statisticsToRemove = _context.Statistics.FirstOrDefault(f => f.Id == footballerToRemove.StatisticsId);
            var accountToRemove = _context.Accounts.FirstOrDefault(f => f.Id == footballerToRemove.AccountId);
            if (statisticsToRemove == null || accountToRemove == null)
                return NotFound();

            _context.Footballers.Remove(footballerToRemove);
            _context.Statistics.Remove(statisticsToRemove);
            _context.Accounts.Remove(accountToRemove);
            _context.SaveChanges();

            // Wersja zoptymalizowana ale nie wiem czy działa jeszcze to zadział ale trzeba zmienić modelBuilder
            /*var footballerToRemove = _context.Footballers.Include(f => f.Statistics).Include(f => f.Account).FirstOrDefault(f => f.Id == id);
            if (footballerToRemove == null)
                return NotFound();

            _context.Footballers.Remove(footballerToRemove);
            _context.SaveChanges();*/

            return RedirectToAction("Index");
        }
    }
}
