using HairSalon.Model.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace HairSalon.Components
{
    public class ServicesViewComponent: ViewComponent
    {
        private IRepositoryOfServices _services;
        public ServicesViewComponent(IRepositoryOfServices repositoryOfServices)
        {
            _services = repositoryOfServices;
        }

        public ViewViewComponentResult Invoke()
        {
            return View(_services.GetAll());
        }
    }
}
