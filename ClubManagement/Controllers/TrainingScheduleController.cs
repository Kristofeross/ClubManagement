using Microsoft.AspNetCore.Mvc;

namespace ClubManagement.Controllers
{
    public class TrainingScheduleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
