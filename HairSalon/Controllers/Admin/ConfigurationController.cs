using HairSalon.Model.Configuration;
using Microsoft.AspNetCore.Mvc;

namespace HairSalon.Controllers.Admin
{
    public class ConfigurationController : Controller
    {
        IRepositoryOfConfiguration _repositoryOfConfiguration;
        public ConfigurationController(IRepositoryOfConfiguration repositoryOfConfiguration)
        {
            _repositoryOfConfiguration = repositoryOfConfiguration;
        }

        public ViewResult Index()
        {
            return View(_repositoryOfConfiguration.GetConfig());
        }

        public RedirectToActionResult SetConfiguration(Config config)
        {
            _repositoryOfConfiguration.SetConfig(config);

            return RedirectToAction("Index");
        }

    }
}
