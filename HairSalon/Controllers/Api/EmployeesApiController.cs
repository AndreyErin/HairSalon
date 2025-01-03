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
        public IActionResult GetAll()
        {
            if(_employees.GetAll().Count == 0)
            {
                return NotFound();
            }

            return Ok(_employees.GetAll());
        }

        [HttpPost]
        public IActionResult Add(Employee employee)
        {
            if ((employee.Name == null) || (employee.Post == null))
            {
                return UnprocessableEntity("Ошибка. Не указанно имя или должность сотрудника. Сотрудник не был добавлен.");
            }

            int result = _employees.Add(employee);
            if (result == 1)
            {
                return Created(); //201
            }
            else
            {
                return Conflict("Ошибка. Сотрудник с таким именем уже есть в базе.");
            }
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
        public IActionResult Delete(int id)
        {
            if(id <= 0)
            {
                return UnprocessableEntity("Ошибка. Некорректный Id сотрудника.");
            }

            int result = _employees.Delete(id);
            if (result == 1)
            {
                return Ok();
            }
            else 
            {
                return NotFound("Ошибка.Сотрудник, с таким ID, не найден.");
            }
        }
    }
}
