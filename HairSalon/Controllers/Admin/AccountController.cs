using Microsoft.AspNetCore.Mvc;

namespace HairSalon.Controllers.Admin
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
    }
}
