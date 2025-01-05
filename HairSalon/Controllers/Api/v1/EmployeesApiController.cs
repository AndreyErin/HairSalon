using HairSalon.Model.Employees;
using Microsoft.AspNetCore.Mvc;

namespace HairSalon.Controllers.Api.v1
{
    [ApiController]
    [Route("api/v1/employees")]
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
            if (_employees.GetAll().Count != 0)
            {
                return Ok(_employees.GetAll());
            }
            else
            {
                return NotFound();
            }           
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult Get(int id)
        {
            if (id <= 0) 
            {
                return UnprocessableEntity("Ошибка. Некорректный id.");
            }

            Employee? result = _employees.Get(id);
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return NotFound("Ошибка. Сотрудник, с таким ID, не найден.");
            }
        }

        [HttpPost]
        public IActionResult Add(Employee employee)
        {
            if (employee.IsValid() == false)
            {
                return UnprocessableEntity("Ошибка. Некорректное имя или должность сотрудника.");
            }

            int result = _employees.Add(employee);
            if (result == 1)
            {
                return Created("", employee); //201
            }
            else
            {
                return Conflict("Ошибка. Сотрудник с таким именем уже есть в базе.");
            }
        }

        [HttpPatch]
        public IActionResult Update(Employee employee)
        {
            if ((employee.IsValid() == false) || (employee.Id <= 0))
            {
                return UnprocessableEntity("Ошибка. Некорректные данные.");
            }

            int result = _employees.Update(employee);
            if (result == 1)
            {
                return Ok(employee);
            }
            else
            {
                return NotFound("Ошибка. Сотрудник с таким id не найден");
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete(int id)
        {
            if (id <= 0)
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
                return NotFound("Ошибка. Сотрудник, с таким ID, не найден.");
            }
        }
    }
}
