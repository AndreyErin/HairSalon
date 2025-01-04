using HairSalon.Model.Configuration;
using Microsoft.AspNetCore.Mvc;

namespace HairSalon.Controllers.Api.v1
{
    [ApiController]
    [Route("api/v1/configuration")]
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
            if ((config == null)
                || (config.NumberOfDaysForRecords <= 0)
                || (config.StartTimeOfDay >= config.EndTimeOfDay))
            {
                return UnprocessableEntity("Ошибка. Некорректные данные.");
            }

            _config.SetConfig(config);
            return Ok(_config.GetConfig());
        }
    }
}
