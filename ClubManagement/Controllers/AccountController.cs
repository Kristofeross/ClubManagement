using Microsoft.AspNetCore.Mvc;
using ClubManagement.ApplicationDbContext;
using ClubManagement.Models;
using BCrypt;
using BCrypt.Net;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Principal;
using Microsoft.AspNetCore.Authorization;

namespace ClubManagement.Controllers
{
    [Route("account")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly ClubDbContext _context;

        public AccountController(ClubDbContext context)
        {
            _context = context;
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            return View("Login");
        }

        [HttpGet("register")]
        public IActionResult Register()
        {
            return View("Register");
        }

        [HttpPost("addAccount")]
        //[Authorize(Policy = "AdminAccess")]
        public IActionResult AddAccount([FromForm] Account model)
        {
            
            if (_context.Accounts.Any(a => a.AccountName == model.AccountName))
            {
                TempData["Alert"] = "Taka nazwa jest już zajęta";
                return RedirectToAction("Register");
            }
            else
            {
                string salt = BCrypt.Net.BCrypt.GenerateSalt();

                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.PasswordHash, salt);
                var account = new Account
                {
                    AccountName = model.AccountName,
                    PasswordHash = hashedPassword,
                    Salt = salt,
                    Role = model.Role
                };

                _context.Accounts.Add(account);
                _context.SaveChanges();

                if (model.Role == "Player")
                    return RedirectToAction("AddPlayer", "Player", new { accountId = account.Id });
                else if (model.Role == "Coach")
                    return RedirectToAction("PrepareToAddCoach", "Coach", new { accountId = account.Id });
                else
                    return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost("loginAccount")]
        public async Task<IActionResult> LoginAccount([FromForm] Account model)
        {
            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.AccountName == model.AccountName);

            if (account != null && BCrypt.Net.BCrypt.Verify(model.PasswordHash, account.PasswordHash))
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, account.AccountName),
                    //new Claim(ClaimTypes.NameIdentifier, account.Email),
                    new Claim(ClaimTypes.Role, account.Role)
                };

                //var accountIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var accountIdentity = new ClaimsIdentity(claims, "login");

                var principal = new ClaimsPrincipal(accountIdentity);

                //await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                await HttpContext.SignInAsync(principal);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["Alert"] = "Błędne dane logowania";
                return RedirectToAction("Login");
            }
        }

        [HttpGet("logoutAccount")]
        [Authorize(Policy = "LoggedInAccess")]
        public async Task<IActionResult> LogoutAccount()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        // Show, edit, delete account

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
                return NotFound();

            var obj = _context.Accounts.FirstOrDefault(x => x.Id == id);

            if (obj == null)
                return NotFound();

            if(type == "accountName")
            {
                ViewBag.editAction = type;
                ViewBag.editTitle = "Zmiana nazwy użytkownika";
            }
            else
            {
                ViewBag.editAction = type;
                ViewBag.editTitle = "Zmiana hasła użytkownika";
            }

            return View(obj);
        }

        [HttpPost("editAccountName")]
        [Authorize(Policy = "LoggedInAccess")]
        public IActionResult EditAccountName([FromForm] Account model)
        {
            if (_context.Accounts.Any(a => a.AccountName == model.AccountName))
            {
                TempData["Alert"] = "Taka nazwa jest już zajęta";
                return RedirectToAction("EditAccount", new { id = model.Id, type = "accountName" });
            }
            else
            {
                var account = _context.Accounts.FirstOrDefault(x => x.Id == model.Id);
                if (account == null)
                    return NotFound();

                account.AccountName = model.AccountName;           

                _context.Accounts.Update(account);
                _context.SaveChanges();

                return RedirectToAction("ShowAccounts");
            }
        }

        [HttpPost("editPassword")]
        [Authorize(Policy = "LoggedInAccess")]
        public IActionResult EditPassword([FromForm] Account model)
        {

            var account = _context.Accounts.FirstOrDefault(x => x.Id == model.Id);
            if (account == null)
                return NotFound();

            if (BCrypt.Net.BCrypt.Verify(model.PasswordHash, account.PasswordHash))
            {
                TempData["Alert"] = "Podane hasło jest takie same jak poprzdnie";
                return RedirectToAction("EditAccount", new { id = model.Id, type = "password" });
            }
            else
            {
                string salt = BCrypt.Net.BCrypt.GenerateSalt();
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.PasswordHash, salt);

                account.PasswordHash = hashedPassword;
            }
                
            _context.Accounts.Update(account);
            _context.SaveChanges();

            return RedirectToAction("ShowAccounts");         
        }

        [HttpGet("removeAccount")]
        [Authorize(Policy = "AdminAccess")]
        public IActionResult RemoveAccount(int id, string filterRole)
        {

            var obj = _context.Accounts.FirstOrDefault(a => a.Id == id);
            if (obj == null)
                return NotFound();

            if(obj.AccountName != HttpContext.User.Identity.Name)
            {
                _context.Accounts.Remove(obj);
                _context.SaveChanges();

                return RedirectToAction("showAccounts", "Account", new { filterRole = filterRole });
            }
            else
            {
                TempData["Alert"] = "Nie możesz usunąć konta na, którym jesteś aktualnie zalogowany";
                return RedirectToAction("ShowAccounts", "Account", new { filterRole = filterRole });
            }
        }
    }
}
