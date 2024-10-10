using HairSalon.Model.Configuration;
using Microsoft.AspNetCore.Mvc;

namespace HairSalon.Controllers
{
    [ApiController]
    [Route("api/configuration")]
    public class ConfigurationApiController : Controller
    {
        IRepositoryOfConfiguration _config;
        public ConfigurationApiController(IRepositoryOfConfiguration configuration) 
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
