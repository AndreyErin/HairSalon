using HairSalon.Model;
using HairSalon.Model.Configuration;
using HairSalon.Model.Records;
using Microsoft.AspNetCore.Mvc;

namespace HairSalon.Controllers
{
    [ApiController]
    [Route("api/records")]
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
        public JsonResult GetAll()
        {
            return Json(new PackageMessage(true, _records.GetAll()));
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

        [HttpGet]
        [Route("forname")]           
        public JsonResult Get(string name)
        {
            PackageMessage packageMessage;

            Record? result = _records.Get(name);
            if (result != null)
            {
                packageMessage = new(true, data: result);
                return Json(packageMessage);
            }

            packageMessage = new(false, errorText: "Ошибка. Запись, с таким именем, не найдена.");
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
        [Route("daysforrecords")]
        public JsonResult GetDaysForRecords()
        {
            return Json(new PackageMessage(true, _records.GetDaysForRecords()));
        }

        [HttpGet]
        [Route("freetimeforrecords")]
        public JsonResult GetFreeTimeForRecords(int timeOfService)
        {
            List<FreeTimeForRecords> freeTimeForRecords = new();
            //время разбивается на отрезки по пол часа
            //просматриваем все даты, начиная от сегодняшней + число дней из конфигурации
            int countDays = _configuration.GetConfig().NumberOfDaysForRecords;
            DateOnly toDay = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            DateOnly lastDay = toDay.AddDays(7);
            var daysForRecords = _records.GetDaysForRecords().Where(d=>d <= lastDay);

            //начало и конец рабочего дня
            TimeOnly startWorkTime = _configuration.GetConfig().StartTimeOfDaty;
            TimeOnly endWorkTime = _configuration.GetConfig().EndTimeOfDaty;

            foreach (var day in daysForRecords) 
            {

                List<TimeOnly> times = new();
                while (startWorkTime < endWorkTime) 
                {
                    //проверяем есть ли записть на это время и дату, если есть то пропускаем ее
                    if (_records.GetAll().FirstOrDefault(r=>r.TimeForVisit == startWorkTime && r.DateForVisit == day) == null) 
                    {
                        //если такой записи нет, то добаялем время в сисок свободного времени
                        times.Add(startWorkTime);
                    }
                    //добавляем 30 минут и проверяем следующее время
                    startWorkTime = startWorkTime.AddMinutes(30);
                }

                freeTimeForRecords.Add(new() { Date = day, Times = times });
                //обнуляем начальное время
                startWorkTime = _configuration.GetConfig().StartTimeOfDaty;
            }

            //формируем ответ
            PackageMessage  packageMessage = new(true, freeTimeForRecords);
            return Json(packageMessage);
        }
    }
}
