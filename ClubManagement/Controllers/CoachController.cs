using ClubManagement.ApplicationDbContext;
using ClubManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClubManagement.Controllers
{
    public class CoachController : Controller
    {
        public readonly ClubDbContext _context;
        public CoachController(ClubDbContext context) 
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult ShowCoaches()
        {
            IEnumerable<Coach> coaches = _context.Coaches.ToList();

            return View(coaches);
        }

        // Dodawanie
        [HttpGet]
        //[Authorize(Policy = "AdminAccess")]
        public IActionResult PrepareToAddCoach(int? accountId) 
        {
            if (accountId == null)
                return RedirectToAction("register", "Account");

            ViewBag.AccountId = accountId;
            return View();
        }

        [HttpPost]
        //[Authorize(Policy = "AdminAccess")]
        public IActionResult AddCoach([FromForm]Coach coach)
        {
            // Walidacja wieku
            DateTime now = DateTime.Now;
            int coachAge = now.Year - coach.DateOfBirth.Year;
            DateTime givenDate = coach.DateOfBirth.AddYears(coachAge); 
            if (givenDate < now)
                coachAge--;

            if (coachAge < 18)
            {
                TempData["Alert"] = "Trener musi mieć co najmniej 18 lat";
                return RedirectToAction("PrepareToAddCoach", new { accountId = coach.AccountId });
            }
            // walidacja głównych trenerów
            string[] arrayOneHeads = { "headCoachFirstTeam", "headCoachReserves", "headCoachJuniors"};

            if(arrayOneHeads.Contains(coach.KindOfCoach))
            {
                var oneHeadCoach = _context.Coaches.Any(c => c.KindOfCoach == coach.KindOfCoach);
                if (oneHeadCoach)
                {
                    TempData["Alert"] = "Trener główny może być tylko jeden";
                    return RedirectToAction("PrepareToAddCoach", new { accountId = coach.AccountId });
                }
            }

            _context.Coaches.Add(coach);
            _context.SaveChanges();

            // Id do account
            var accountId = _context.Accounts.FirstOrDefault(a => a.Id == coach.AccountId);
            if (accountId == null)
                return NotFound();

            accountId.CoachId = coach.Id;
            _context.Accounts.Update(accountId);
            _context.SaveChanges();

            return RedirectToAction("ShowCoaches");
        }

        // Edytowanie
        [HttpGet]
        public IActionResult PrepareToEditCoach(int? id)
        {
            if (id == null || id == 0)
                return NotFound();

            var obj = _context.Coaches.FirstOrDefault(f => f.Id == id);

            if (obj == null)
                return NotFound();

            return View(obj);
        }

        [HttpPost]
        //[Authorize(Policy = "AdminAccess")]
        public IActionResult EditCoach([FromForm]Coach coach)
        {
            // Walidacja wieku
            DateTime now = DateTime.Now;
            int coachAge = now.Year - coach.DateOfBirth.Year;
            DateTime givenDate = coach.DateOfBirth.AddYears(coachAge);
            if (givenDate < now)
                coachAge--;

            if (coachAge < 18)
            {
                TempData["Alert"] = "Trener musi mieć co najmniej 18 lat";
                return RedirectToAction("PrepareToEditCoach", new { id = coach.Id });
            }

            // walidacja do roli trenera

            var coachToEdit = _context.Coaches.FirstOrDefault(x => x.Id == coach.Id);
            if (coachToEdit == null)
                return NotFound();

            string[] arrayOneHeads = { "headCoachFirstTeam", "headCoachReserves", "headCoachJuniors" };
            if (arrayOneHeads.Contains(coach.KindOfCoach))
            {
                var oneHeadCoach = _context.Coaches.Any(c => c.KindOfCoach == coach.KindOfCoach && c.Id != coach.Id);
                if (oneHeadCoach)
                {
                    TempData["Alert"] = "Trener główny może być tylko jeden";
                    return RedirectToAction("PrepareToEditCoach", new { id = coach.Id });
                }
            }

            coachToEdit.Name = coach.Name;
            coachToEdit.LastName = coach.LastName;
            coachToEdit.Country = coach.Country;
            coachToEdit.KindOfCoach = coach.KindOfCoach;
            coachToEdit.DateOfBirth = coach.DateOfBirth;

            _context.SaveChanges();

            TempData["Success"] = "Pomyślnie zaktualizowano trenera";
            return RedirectToAction("ShowCoaches");
        }

        // usuwanie
        public IActionResult RemoveCoach(int? id) 
        { 
            if (id == null)
                return NotFound("RemoveCoach - brak id");

            var coachToRemove = _context.Coaches.FirstOrDefault(c => c.Id == id);
            if (coachToRemove == null)
                return NotFound("RemoveCoach - brak obiektu trenera w bazie do usunięcia");

            _context.Coaches.Remove(coachToRemove);

            var accountToRemove = _context.Accounts.FirstOrDefault(a => a.Id == coachToRemove.AccountId);
            if (accountToRemove != null)
                _context.Accounts.Remove(accountToRemove);

            _context.SaveChanges();

            return RedirectToAction("ShowCoaches"); 
        }
    }
}
