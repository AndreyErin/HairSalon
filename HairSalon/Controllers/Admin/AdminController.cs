using Microsoft.AspNetCore.Mvc;

namespace HairSalon.Controllers.Admin
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
