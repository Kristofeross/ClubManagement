﻿using Microsoft.AspNetCore.Mvc;
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
        [Authorize(Policy = "AdminAccess")]
        public IActionResult AddAccount([FromForm] Account model)
        {
            if (_context.Accounts.Any(a => a.Email == model.Email))
            {
                TempData["Alert"] = "Taki email jest już zajęty";
                return RedirectToAction("Register");
            }
            else
            {
                string salt = BCrypt.Net.BCrypt.GenerateSalt();

                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.PasswordHash, salt);

                _context.Accounts.Add(new Account
                {
                    Email = model.Email,
                    PasswordHash = hashedPassword,
                    Salt = salt,
                    Role = model.Role
                    //Role = model.Role
                });
                _context.SaveChanges();

                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost("loginAccount")]
        public async Task<IActionResult> LoginAccount([FromForm] Account model)
        {
            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Email == model.Email);

            if (account != null && BCrypt.Net.BCrypt.Verify(model.PasswordHash, account.PasswordHash))
            {
                var claims = new List<Claim>
                {
                    //new Claim(ClaimTypes.Name, account.Email),
                    new Claim(ClaimTypes.NameIdentifier, account.Email),
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
        public async Task<IActionResult> LogoutAccount()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

    }
}
