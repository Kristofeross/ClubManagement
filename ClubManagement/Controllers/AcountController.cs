/*using Microsoft.AspNetCore.Mvc;
using ClubManagement.Models;
using ClubManagement.Services;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;


namespace ClubManagement.Controllers
{
    [Route("api/acount")]
    [ApiController]
    public class AcountController : Controller
    {
        private readonly IAccountService _accountService;

        public AcountController(IAccountService acountService)
        {
            _accountService = acountService;
        }

        [HttpPost("register")]
        public IActionResult RegisterUser([FromBody] AccountRegisterDto dto)
        {
            _accountService.AccountRegister(dto);
            return Ok();
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] AccountLoginDto dto)
        {
            string token = _accountService.GenerateJwt(dto);
            return Ok(token);
        }
    }
}
*/