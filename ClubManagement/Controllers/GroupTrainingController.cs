using Microsoft.AspNetCore.Mvc;

namespace ClubManagement.Controllers
{
    public class GroupTrainingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
