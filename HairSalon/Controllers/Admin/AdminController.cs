using Microsoft.AspNetCore.Mvc;
using System;

namespace HairSalon.Controllers.Admin
{
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
