using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using HairSalon.Model.Authorization.Api_Jwt;
using Microsoft.AspNetCore.Identity;
using HairSalon.Model.Authorization.Admin;

namespace HairSalon.Controllers.Api.v1
{
    public class AccountApiController : Controller
    {
        UserManager<User> _userManager;
        public AccountApiController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        [Route("api/v1/token")]
        public async Task<IActionResult> Token(string name, string password)
        {
            try
            {
                if (await UserVerification(name, password) == false)
                {
                    return BadRequest("Ошибка. Неверное имя или пароль.");
                }

                var claims = new List<Claim> { new Claim(ClaimTypes.Name, "AdminHS") };
                // создаем JWT-токен
                var jwt = new JwtSecurityToken(
                    issuer: AuthJwtOptions.ISSUER,
                    audience: AuthJwtOptions.AUDIENCE,
                    claims: claims,
                    expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(AuthJwtOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthJwtOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

                return Ok(new JwtSecurityTokenHandler().WriteToken(jwt));
            }
            catch (Exception)
            {
                return BadRequest("Ошибка сервера.");
            }
        }

        private async Task<bool> UserVerification(string name, string password)
        {
            User? user = await _userManager.FindByNameAsync(name);
            if (user != null)
            {
                bool result = await _userManager.CheckPasswordAsync(user, password);

                if (result) 
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else 
            {
                return false;
            }
        }
    }
}
