using ClubManagement.ApplicationDbContext;
using ClubManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ClubManagement.Controllers
{
    public class UserManagementController : Controller
    {
        private readonly ClubDbContext _context;
        public UserManagementController(ClubDbContext context)
        {
            _context = context;
        }

        [HttpGet("showAccount")]
        [Authorize(Policy = "LoggedInAccess")]
        public IActionResult ShowAccount()
        {
            string userName = HttpContext.User.Identity.Name;

            var account = _context.Accounts.FirstOrDefault(a => a.AccountName == userName);

            if (account == null)
                return NotFound("Nie znaleziono Usera");

            return View(account);
        }

        [HttpGet("showAccounts")]
        [Authorize(Policy = "AdminAccess")]
        public IActionResult ShowAccounts(string? filterRole = "all")
        {
            ViewBag.FilterRole = filterRole;

            IEnumerable<Account> accounts;

            if (filterRole != "all")
                accounts = _context.Accounts
                    .Where(a => a.Role == filterRole);
            else
                accounts = _context.Accounts.OrderBy(a => a.Role);

            return View(accounts);
        }

        [HttpGet("editAccount")]
        [Authorize(Policy = "LoggedInAccess")]
        public IActionResult EditAccount(int? id, string? type)
        {
            if (id == null || id == 0 || type == null || type == "")
                return NotFound("Nie ma id albo typu");

            var obj = _context.Accounts.FirstOrDefault(x => x.Id == id);

            if (obj == null)
                return NotFound("Nie znaleziono w bazie");

            /*if (type == "accountName")
            {
                ViewBag.editAction = type;
                ViewBag.editTitle = "Zmiana nazwy użytkownika";
            }
            else if(type == "email")
            {
                ViewBag.editAction = type;
                ViewBag.editTitle = "Zmiana emaila użytkownika";
            }*/
            if (type == "email")
            {
                ViewBag.editAction = type;
                ViewBag.editTitle = "Zmiana emaila użytkownika";
            }
            else
            {
                ViewBag.editAction = type;
                ViewBag.editTitle = "Zmiana hasła użytkownika";
            }

            return View(obj);
        }

        /*[HttpPost("editAccountName")]
        [Authorize(Policy = "LoggedInAccess")]
        public IActionResult EditAccountName([FromForm] Account model)
        {
            var account = _context.Accounts.FirstOrDefault(x => x.Id == model.Id);
            if (account == null)
                return NotFound();

            if(account.AccountName != model.AccountName)
            {
                if (_context.Accounts.Any(a => a.AccountName == model.AccountName))
                {
                    TempData["Alert"] = "Taka nazwa jest już zajęta";
                    return RedirectToAction("EditAccount", new { id = model.Id, type = "accountName" });
                }
            }

            account.AccountName = model.AccountName;

            _context.Accounts.Update(account);
            _context.SaveChanges();

            return RedirectToAction("ShowAccount");
        }*/

        [HttpPost("editAccountEmail")]
        [Authorize(Policy = "LoggedInAccess")]
        public IActionResult EditAccountEmail([FromForm] Account model)
        {
            var account = _context.Accounts.FirstOrDefault(x => x.Id == model.Id);
            if (account == null)
                return NotFound();

            if(account.Email != model.Email)
            {
                if (_context.Accounts.Any(a => a.Email == model.Email))
                {
                    TempData["Alert"] = "Taki email jest już zajęty";
                    return RedirectToAction("EditAccount", new { id = model.Id, type = "email" });
                }
            }

            account.Email = model.Email;

            _context.Accounts.Update(account);
            _context.SaveChanges();

            return RedirectToAction("ShowAccount");
        }

        [HttpPost("editPassword")]
        [Authorize(Policy = "LoggedInAccess")]
        public IActionResult EditPassword([FromForm] Account model, [FromForm] string oldPassword)
        {

            var account = _context.Accounts.FirstOrDefault(x => x.Id == model.Id);
            if (account == null)
                return NotFound();

            if (!BCrypt.Net.BCrypt.Verify(oldPassword, account.PasswordHash))
            {
                TempData["Alert"] = "Podane hasło jest niepoprawne";
                return RedirectToAction("EditAccount", new { id = model.Id, type = "password" });
            }

            string salt = BCrypt.Net.BCrypt.GenerateSalt();
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.PasswordHash, salt);
            account.PasswordHash = hashedPassword;

            _context.Accounts.Update(account);
            _context.SaveChanges();

            return RedirectToAction("ShowAccount");
        }

        [HttpGet("removeAccount")]
        [Authorize(Policy = "AdminAccess")]
        public IActionResult RemoveAccount(int id, string filterRole)
        {

            var obj = _context.Accounts.FirstOrDefault(a => a.Id == id);
            if (obj == null)
                return NotFound();

            _context.Accounts.Remove(obj);
            _context.SaveChanges();

            if (obj.AccountName == HttpContext.User.Identity.Name)
                return RedirectToAction("LogoutAccount", "Account");

            return RedirectToAction("ShowAccounts", new { filterRole = filterRole });
        }
    }
}
