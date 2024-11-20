using HairSalon.Model.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace HairSalon.Components
{
    public class ServicesViewComponent: ViewComponent
    {
        private IRepositoryOfServices _repositoryOfServices;
        public ServicesViewComponent(IRepositoryOfServices services)
        {
            _repositoryOfServices = services;
        }

        public ViewViewComponentResult Invoke()
        {
            return View(_repositoryOfServices.GetAll());
        }
    }
}
