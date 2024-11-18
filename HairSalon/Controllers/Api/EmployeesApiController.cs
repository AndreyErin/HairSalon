using HairSalon.Model;
using HairSalon.Model.Employees;
using Microsoft.AspNetCore.Mvc;

namespace HairSalon.Controllers.Api
{
    [ApiController]
    [Route("api/employee")]
    public class EmployeesApiController : Controller
    {
        private IRepositoryOfEmployees _employees;
        public EmployeesApiController(IRepositoryOfEmployees employees)
        {
            _employees = employees;
        }

        [HttpGet]
        public JsonResult GetAll()
        {
            PackageMessage packageMessage = new(true, _employees.GetAll());
            return Json(packageMessage);
        }

        [HttpPost]
        public JsonResult Add(Employee employee)
        {
            PackageMessage packageMessage;
            int result = _employees.Add(employee);

            if (result == 1)
            {
                packageMessage = new(true);
                return Json(packageMessage);
            }

            packageMessage = new(false, errorText: "Ошибка. Сотрудник не был добавлен.");
            return Json(packageMessage);

        }

        [HttpPatch]
        public JsonResult Update(Employee employee)
        {
            PackageMessage packageMessage;
            int result = _employees.Update(employee);

            if (result == 1)
            {
                packageMessage = new(true);
                return Json(packageMessage);
            }

            packageMessage = new(false, errorText: "Ошибка. Информация о сотруднике не была изменена.");
            return Json(packageMessage);
        }

        [HttpGet]
        [Route("{id}")]
        public JsonResult Get(int id)
        {
            PackageMessage packageMessage;
            Employee? result = _employees.Get(id);

            if (result != null)
            {
                packageMessage = new(true, result);
                return Json(packageMessage);
            }

            packageMessage = new(false, errorText: "Ошибка. Сотрудник, с таким ID, не найден.");
            return Json(packageMessage);
        }


        [HttpGet]
        [Route("forname")]
        public JsonResult Get(string name)
        {
            PackageMessage packageMessage;
            Employee? result = _employees.Get(name);

            if (result != null)
            {
                packageMessage = new(true, result);
                return Json(packageMessage);
            }

            packageMessage = new(false, errorText: "Ошибка. Сотрудник, с таким именем, не найден.");
            return Json(packageMessage);
        }

        [HttpDelete]
        [Route("{id}")]
        public JsonResult Delete(int id)
        {
            PackageMessage packageMessage;
            int result = _employees.Delete(id);

            if (result == 1)
            {
                packageMessage = new(true);
                return Json(packageMessage);
            }

            packageMessage = new(false, errorText: "Ошибка. Сотрудник не был удален. (Сотрудник, с таким ID, не найден.)");
            return Json(packageMessage);
        }
    }
}
