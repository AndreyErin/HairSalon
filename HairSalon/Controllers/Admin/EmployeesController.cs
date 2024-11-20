using HairSalon.Model.Employees;
using Microsoft.AspNetCore.Mvc;

namespace HairSalon.Controllers.Admin
{
    public class EmployeesController : Controller
    {
        private IRepositoryOfEmployees _employees;
        public EmployeesController(IRepositoryOfEmployees repositoryOfEmployees) 
        {
            _employees = repositoryOfEmployees;
        }
        public IActionResult Index()
        {
            return View(_employees.GetAll());
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
            _employees.Add(employee);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ViewResult Edit(int id) 
        {
            Employee employee = _employees.Get(id);
            ViewBag.Title = "Изменить";
            return View("AddOrEdit", employee);
        }

        [HttpPost]
        public RedirectToActionResult Edit(Employee employee)
        {
            _employees.Update(employee);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public RedirectToActionResult Delete(int id) 
        {
            _employees.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
