using Microsoft.AspNetCore.Mvc;

namespace ClubManagement.Controllers
{
    public class CoachController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
