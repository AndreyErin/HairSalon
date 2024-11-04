using HairSalon.Model.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace HairSalon.Components
{
    public class ConfigurationViewComponent: ViewComponent
    {
        private IRepositoryOfConfiguration _repositoryOfConfiguration;

        public ConfigurationViewComponent(IRepositoryOfConfiguration repositoryOfConfiguration)
        {
            _repositoryOfConfiguration = repositoryOfConfiguration;
        }
        public ViewViewComponentResult Invoke() 
        {
            return View(_repositoryOfConfiguration.GetConfig()); 
        }
    }
}
