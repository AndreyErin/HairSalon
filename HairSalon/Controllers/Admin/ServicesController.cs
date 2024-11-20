using Microsoft.AspNetCore.Mvc;

namespace HairSalon.Controllers.Admin
{
    public class ServicesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
