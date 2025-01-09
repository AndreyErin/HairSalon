using HairSalon.Model.Configuration;
using HairSalon.Model.Records;
using HairSalon.Model.Records.Api;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HairSalon.Controllers.Api.v1
{
    [ApiController]
    [Route("api/v1/records")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RecordsApiController : Controller
    {
        IRepositoryOfRecords _records;
        IRepositoryOfConfiguration _configuration;
        public RecordsApiController(IRepositoryOfRecords records, IRepositoryOfConfiguration configuration)
        {
            _records = records;
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            if(_records.GetAll().Count != 0)
            {
                return Ok(_records.GetAll());
            }
            else
            {
                return NotFound("Ошибка. Записи не найдены.");
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

            Record? result = _records.Get(id);
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return NotFound("Ошибка. Запись, с таким ID, не найдена.");
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete(int id)
        {
            if (id <= 0)
            {
                return UnprocessableEntity("Ошибка. Некорректный id.");
            }

            int result = _records.Delete(id);
            if (result == 1)
            {                
                return Ok();
            }
            else
            {
                return NotFound("Ошибка. Запись, с таким ID, не найдена.");
            }
        }

        [HttpPost]
        public IActionResult Add(Record record)
        {
            if (record.IsValid() == false)
            {
                return UnprocessableEntity("Ошибка. Некорректные данные.");
            }

            int result = _records.Add(record);
            if (result == 1)
            {
                return Ok(record);
            }
            else
            {
                return Conflict("Ошибка. Выбранное время уже занято.");
            }
        }

        [HttpGet]
        [Route("workdays")]
        public IActionResult GetDaysForRecords()
        {
            if (_records.GetDaysForRecords().Count > 0)
            {
                return Ok(_records.GetDaysForRecords());
            }
            else
            {
                return NotFound("Ошибка. Нет свободных дней для записи");
            }
        }

        [HttpGet]
        [Route("worktimes")]
        public IActionResult GetFreeTimeForRecords(int timeOfService, int employeeId)
        {
            if (timeOfService <= 0 || employeeId <= 0)
            {
                return UnprocessableEntity("Ошибка. Некорректные данные.");
            }

            //создаем сервис, который будет расчитывать свободное время для записи
            FreeTimeForRecordService freeTimeService = new(_records, _configuration);

            List<FreeTimeForRecords> freeTimes = freeTimeService.GetFreeTimes(timeOfService, employeeId);

            //формируем ответ
            if (freeTimes.Count > 0)//время для записи есть
            {
                return Ok(freeTimes);
            }
            else
            {
                return NotFound("Нет свободного времени для записи");
            }
        }
    }
}
