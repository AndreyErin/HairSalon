using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HairSalon.Controllers.Admin
{
    [Authorize]
    [AutoValidateAntiforgeryToken]
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public ViewResult ErrorPage(string errorMessage)
        {
            ViewBag.ReturnUrl = Request.Headers["Referer"];
            ViewBag.ErrorText = Uri.UnescapeDataString(errorMessage);

            return View();
        }
    }
}
