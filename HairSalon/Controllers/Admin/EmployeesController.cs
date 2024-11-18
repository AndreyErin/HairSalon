using HairSalon.Model.Employees;
using Microsoft.AspNetCore.Mvc;

namespace HairSalon.Controllers.Admin
{
    public class EmployeesController : Controller
    {
        private IRepositoryOfEmployees _repositoryOfEmployees;
        public EmployeesController(IRepositoryOfEmployees repository) 
        {
            _repositoryOfEmployees = repository;
        }
        public IActionResult Index()
        {
            return View(_repositoryOfEmployees.GetAll());
        }
    }
}
