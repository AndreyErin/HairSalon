using HairSalon.Model;
using HairSalon.Model.Configuration;
using HairSalon.Model.Records;
using HairSalon.Model.Records.Api;
using Microsoft.AspNetCore.Mvc;

namespace HairSalon.Controllers.Api.v1
{
    [ApiController]
    [Route("api/v1/records")]
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
                return NotFound();
            }
        }

        [HttpGet]
        [Route("{id}")]
        public JsonResult Get(int id)
        {
            PackageMessage packageMessage;

            Record? result = _records.Get(id);
            if (result != null)
            {
                packageMessage = new(true, data: result);
                return Json(packageMessage);
            }

            packageMessage = new(false, errorText: "Ошибка. Запись, с таким ID, не найдена.");
            return Json(packageMessage);
        }

        [HttpDelete]
        [Route("{id}")]
        public JsonResult Delete(int id)
        {
            PackageMessage packageMessage;

            int result = _records.Delete(id);
            if (result == 1)
            {
                packageMessage = new(true);
                return Json(packageMessage);
            }

            packageMessage = new(false, errorText: "Ошибка. Запись не была удалена.");
            return Json(packageMessage);
        }

        [HttpPost]
        public JsonResult Add(Record record)
        {
            PackageMessage packageMessage;

            int result = _records.Add(record);
            if (result == 1)
            {
                packageMessage = new(true);
                return Json(packageMessage);
            }

            packageMessage = new(false, errorText: "Ошибка. Выбранное время уже занято.");
            return Json(packageMessage);
        }

        [HttpGet]
        [Route("days")]
        public JsonResult GetDaysForRecords()
        {
            return Json(new PackageMessage(true, _records.GetDaysForRecords()));
        }

        [HttpGet]
        [Route("times")]
        public JsonResult GetFreeTimeForRecords(int timeOfService, int employeeId)
        {
            //создаем сервис, который будет расчитывать свободное время для записи
            FreeTimeForRecordService freeTimeService = new(_records, _configuration);

            List<FreeTimeForRecords> freeTime = freeTimeService.GetFreeTimes(timeOfService, employeeId);

            //формируем ответ
            if (freeTime.Count > 0)//время для записи есть
            {
                PackageMessage packageMessage = new(true, freeTime);
                return Json(packageMessage);
            }
            else
            {
                PackageMessage packageMessage = new(false, null, "Нет свободного времени для записи");
                return Json(packageMessage);
            }
        }
    }
}
