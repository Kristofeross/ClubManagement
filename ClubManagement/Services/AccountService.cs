/*using ClubManagement.ApplicationDbContext;
using ClubManagement.Exceptions;
using ClubManagement.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace ClubManagement.Services
{
    public interface IAccountService
    {
        void AccountRegister(AccountRegisterDto dto);
        string GenerateJwt(AccountLoginDto dto);
    }
    public class AccountService: IAccountService
    {
        private readonly ClubDbContext _dbContext;
        private readonly IPasswordHasher<Account> _passwordHasher;
        private readonly AuthenticationSettings _authenticationSettings;

        public AccountService(ClubDbContext dbContext, IPasswordHasher<Account> passwordHasher, AuthenticationSettings authenticationSettings)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
            _authenticationSettings = authenticationSettings;
        }
        public void AccountRegister(AccountRegisterDto dto)
        {
            var newAcount = new Account()
            {
                *//*AccountName = dto.AccountName,*//*
                Email = dto.Email,
                RoleId = dto.RoleId,
            };

            var hashedPassword = _passwordHasher.HashPassword(newAcount, dto.PasswordHash);

            newAcount.PasswordHash = hashedPassword;
            _dbContext.Accounts.Add(newAcount);
            _dbContext.SaveChanges();
        }
        // Do zrobienia tego asynchronicznie
        public string GenerateJwt(AccountLoginDto dto)
        {
            var user = _dbContext.Accounts
                .Include(u => u.Role)
                .FirstOrDefault(u => u.Email == dto.Email);

            if (user is null)
            {
                throw new BadRequestException("Invalid username or password");
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if(result == PasswordVerificationResult.Failed)
            {
                throw new BadRequestException("Invalid username or password");
            }

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                *//*new Claim(ClaimTypes.Name, user.AccountName),*//*
                new Claim(ClaimTypes.Role, user.Role.Name),
                // tego nie ma to pokazuje jak dodaje sie własne claimy
                *//*new Claim("DateOfBirth", user.DateOfBirth.Value.ToString("yyyy-MM-dd"))*//* 
            };

            // generowanie klucza na bazie klucza Jwtkey
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
            // podanie klucza oraz zakodowanie go danym algorytmem
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            // obliczenie podanie daty do kiedy bedzei poprawny
            var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);

            // utworzenie tokenu
            var token = new JwtSecurityToken(
                _authenticationSettings.JwtIssuer,
                _authenticationSettings.JwtIssuer,
                claims,
                expires,
                signingCredentials: cred
            );

            // Wygenerowanie tokenu do typu string
            var tokenHandler = new JwtSecurityTokenHandler();

            return tokenHandler.WriteToken(token);

        }
    }
}
*/