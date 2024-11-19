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

        [HttpGet]
        public ViewResult Add()
        {
            ViewBag.Title = "Добавить";
            return View("AddOrEdit");
        }

        [HttpPost]
        public RedirectToActionResult Add(Employee employee) 
        {
            _repositoryOfEmployees.Add(employee);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ViewResult Edit(int id) 
        {
            Employee employee = _repositoryOfEmployees.Get(id);
            ViewBag.Title = "Изменить";
            return View("AddOrEdit", employee);
        }

        [HttpPost]
        public RedirectToActionResult Edit(Employee employee)
        {
            _repositoryOfEmployees.Update(employee);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public RedirectToActionResult Delete(int id) 
        {
            _repositoryOfEmployees.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
