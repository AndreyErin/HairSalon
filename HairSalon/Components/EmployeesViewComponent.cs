using HairSalon.Model.Employees;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace HairSalon.Components
{
    public class EmployeesViewComponent: ViewComponent
    {
        IRepositoryOfEmployees _repositoryOfEmployees;
        public EmployeesViewComponent(IRepositoryOfEmployees repository)
        {
            _repositoryOfEmployees = repository;
        }

        public ViewViewComponentResult Invoke() 
        { 
            return View(_repositoryOfEmployees.GetAll());
        }
    }
}
