using HairSalon.Model.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HairSalon.Controllers.Api.v1
{
    [ApiController]
    [Route("api/v1/configuration")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ConfigurationApiController : Controller
    {
        IRepositoryOfConfiguration _config;
        public ConfigurationApiController(IRepositoryOfConfiguration configuration)
        {
            _config = configuration;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_config.GetConfig());
        }

        [HttpPatch]
        public IActionResult Set(Config config)
        {
            if (config.IsValid() == false)
            {
                return UnprocessableEntity("Ошибка. Некорректные данные.");
            }

            _config.SetConfig(config);
            return Ok(_config.GetConfig());
        }
    }
}
