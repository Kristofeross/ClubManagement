using Microsoft.AspNetCore.Mvc;

namespace ClubManagement.Controllers
{
    public class PlayerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
