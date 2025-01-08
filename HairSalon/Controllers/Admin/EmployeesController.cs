using HairSalon.Model.Employees;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HairSalon.Controllers.Admin
{
    [Authorize]
    [AutoValidateAntiforgeryToken]
    public class EmployeesController : Controller
    {
        private IRepositoryOfEmployees _employees;
        public EmployeesController(IRepositoryOfEmployees repositoryOfEmployees) 
        {
            _employees = repositoryOfEmployees;
        }
        public ViewResult Index()
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
            int result =  _employees.Add(employee);
            if (result == 1)
            {
                return RedirectToAction("Index");
            }
            else
            {
                string errorMessage = Uri.EscapeDataString("Не удалось добавить сотрудника.");
                return RedirectToAction("ErrorPage","Admin", new {errorMessage});
            }          
        }

        [HttpGet]
        public ViewResult Edit(int id) 
        {
            Employee employee = _employees.Get(id) ?? new Employee();
            ViewBag.Title = "Изменить";
            return View("AddOrEdit", employee);
        }

        [HttpPost]
        public RedirectToActionResult Edit(Employee employee)
        {

            int result = _employees.Update(employee);

            if (result == 1)
            {
                return RedirectToAction("Index");
            }
            else
            {
                string errorMessage = Uri.EscapeDataString("Не удалось изменить информацию о сотруднике.");
                return RedirectToAction("ErrorPage", "Admin", new { errorMessage });
            }
        }

        [HttpPost]
        public RedirectToActionResult Delete(int id) 
        {
            int result = _employees.Delete(id);
            if (result == 1)
            {
                return RedirectToAction("Index");
            }
            else
            {
                string errorMessage = Uri.EscapeDataString("Не удалось удалить сотрудника.");
                return RedirectToAction("ErrorPage", "Admin", new { errorMessage });
            }
        }
    }
}
