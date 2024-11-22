using HairSalon.Model.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace HairSalon.Components
{
    public class ConfigurationViewComponent: ViewComponent
    {
        private IRepositoryOfConfiguration _configuration;

        public ConfigurationViewComponent(IRepositoryOfConfiguration repositoryOfConfiguration)
        {
            _configuration = repositoryOfConfiguration;
        }
        public ViewViewComponentResult Invoke() 
        {
            return View(_configuration.GetConfig()); 
        }
    }
}
