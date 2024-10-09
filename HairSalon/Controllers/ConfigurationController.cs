using HairSalon.Model.Configuration;
using Microsoft.AspNetCore.Mvc;

namespace HairSalon.Controllers
{
    [ApiController]
    [Route("api/configuration")]
    public class ConfigurationController : Controller
    {
        IRepositoryOfConfiguration _config;
        public ConfigurationController(IRepositoryOfConfiguration configuration) 
        {
            _config = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            return Json(_config.GetConfig());
        }

        [HttpPost]
        public void Set(Config config) 
        {
            _config.SetConfig(config);
        }
    }
}
