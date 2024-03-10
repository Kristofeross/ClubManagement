using Microsoft.AspNetCore.Mvc;

namespace ClubManagement.Controllers
{
    public class PlanController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
