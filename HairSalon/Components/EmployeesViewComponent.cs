using HairSalon.Model.Employees;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace HairSalon.Components
{
    public class EmployeesViewComponent: ViewComponent
    {
        IRepositoryOfEmployees _employees;
        public EmployeesViewComponent(IRepositoryOfEmployees repositoryOfEmployees)
        {
            _employees = repositoryOfEmployees;
        }

        public ViewViewComponentResult Invoke() 
        { 
            return View(_employees.GetAll());
        }
    }
}
