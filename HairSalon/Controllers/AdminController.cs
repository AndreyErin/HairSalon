﻿using Microsoft.AspNetCore.Mvc;

namespace HairSalon.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}