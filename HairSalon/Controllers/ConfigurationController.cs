using HairSalon.Model.Configuration;
using Microsoft.AspNetCore.Mvc;

namespace HairSalon.Controllers
{
    public class ConfigurationController: Controller
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
    }
}
