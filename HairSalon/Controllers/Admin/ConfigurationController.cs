using HairSalon.Model.Configuration;
using Microsoft.AspNetCore.Mvc;

namespace HairSalon.Controllers.Admin
{
    public class ConfigurationController : Controller
    {
        private IRepositoryOfConfiguration _configuration;
        public ConfigurationController(IRepositoryOfConfiguration repositoryOfConfiguration)
        {
            _configuration = repositoryOfConfiguration;
        }

        public ViewResult Index()
        {
            return View(_configuration.GetConfig());
        }

        public RedirectResult SetConfiguration(Config config)
        {
            _configuration.SetConfig(config);

            return Redirect("~/Admin");
        }

    }
}
