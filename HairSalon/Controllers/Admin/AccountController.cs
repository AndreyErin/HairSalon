using HairSalon.Model.Authorization.Admin;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HairSalon.Controllers.Admin
{
    public class AccountController : Controller
    {
        private SignInManager<User> _signInManager;
        public AccountController(SignInManager<User> signInManager)
        {
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login(string ReturnUrl)
        {
            if (ReturnUrl != null) 
            {
                //если в запросе есть url возврата
                TempData["ReturnUrl"] = ReturnUrl;
            }
            else
            {
                //если это не первая поптытка входа и url из запроса уже потерялся,
                //то пересохраняем его
                TempData.Keep();
            }

            return View();
        }

        //Admin - Admin123$
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            var ReturnUrl = TempData["ReturnUrl"] as string;

            var resutl = await _signInManager.PasswordSignInAsync(loginModel.Name, loginModel.Password,false,false);
            if (resutl.Succeeded) 
            {
                return Redirect(ReturnUrl ?? "/Admin");
            }
            else
            {
                TempData.Keep();
                ModelState.AddModelError("", "Неверный логин или пароль.");
                return View("Login");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Login", "Account");
        }

    }
}
