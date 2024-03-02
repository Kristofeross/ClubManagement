using Microsoft.AspNetCore.Mvc;

namespace ClubManagement.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
