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
        public JsonResult GetFreeTimeForRecords(int timeOfService, int employeeId)
        {
            //время разбивается на отрезки по пол часа
            int extraTimeLags;//количество дополнительных отрезков по  30 минут

            switch (timeOfService)
            {
                //если время укладывается в 30 минут
                case <=30:
                    extraTimeLags = 0;
                    break;
                //если время больше 30 минут
                case > 30:
                    extraTimeLags = (timeOfService -30) / 30;
                    if (timeOfService % 30 != 0)
                        extraTimeLags++;
                    break;
            }


            List<FreeTimeForRecords> freeTimeForRecords = new();
            //просматриваем все даты, начиная от сегодняшней + число дней из конфигурации
            int countDays = _configuration.GetConfig().NumberOfDaysForRecords;
            DateOnly toDay = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            DateOnly lastDay = toDay.AddDays(7);
            var daysForRecords = _records.GetDaysForRecords().Where(d=>d >=toDay && d <= lastDay);

            //начало и конец рабочего дня
            TimeOnly startWorkTime = _configuration.GetConfig().StartTimeOfDaty;
            TimeOnly endWorkTime = _configuration.GetConfig().EndTimeOfDaty;

            foreach (var day in daysForRecords) 
            {
                //если эта дата сегодня и часть рабочего времени уже прошла
                //, то корректируем стартовое время от текущего
                if(DateTime.Now.ToShortDateString() == day.ToShortDateString() &&
                    DateTime.Now.Hour >= startWorkTime.Hour) 
                {
                    startWorkTime = new(DateTime.Now.Hour +1, 0);
                }


                List<TimeOnly> times = new();
                while (startWorkTime < endWorkTime) 
                {
                    //проверяем есть ли записть на это время и дату(у конкретного работника)
                    //, если нет то добавляем это время в список доступных
                    if (_records.GetAll().FirstOrDefault(r=>
                        r.TimeForVisit == startWorkTime &&
                        r.DateForVisit == day &&
                        r.EmployeeId == employeeId) == null) 
                    {
                        switch (extraTimeLags)
                        {
                            case 0:
                                //если такой записи нет и она укалдывается в 30 минут, то добаялем время в сисок свободного времени
                                times.Add(startWorkTime);
                                break;
                            case > 0:
                                //проверяем свободно ли дополнительное время, необходимое для закписи
                                if(CheckExtraTime(startWorkTime, endWorkTime, day, extraTimeLags, employeeId))
                                    times.Add(startWorkTime);
                                break;
                        }


                    }
                    //добавляем 30 минут и проверяем следующее время
                    startWorkTime = startWorkTime.AddMinutes(30);
                }

                //если в этот день доступна хотябы одна запись, то добавляем этот день
                if(times.Count > 0)
                {
                    freeTimeForRecords.Add(new() { Date = day, Times = times });
                }
                
                //обнуляем начальное время
                startWorkTime = _configuration.GetConfig().StartTimeOfDaty;
            }

            //формируем ответ
            if (freeTimeForRecords.Count > 0)//время для записи есть
            {
                PackageMessage packageMessage = new(true, freeTimeForRecords);
                return Json(packageMessage);
            }
            else 
            {
                PackageMessage packageMessage = new(false, null, "Нет свободного времени для записи");
                return Json(packageMessage);
            }

        }

        public bool CheckExtraTime(TimeOnly startTime, TimeOnly endTime, DateOnly day, int extraTimeCount, int employeeId)
        {
            for (int i = 0; i < extraTimeCount; i++)
            {
                startTime = startTime.AddMinutes(30);
                if (startTime == endTime)
                {
                    return false; //если мы выходим за пределы рабочего времени
                }


                if (_records.GetAll().FirstOrDefault(r =>
                        r.TimeForVisit == startTime &&
                        r.DateForVisit == day &&
                        r.EmployeeId == employeeId) != null)
                {
                    return false; //если такая запись уже есть в базе
                }
                
            }

            return true; //если все дополнительные временные отрезки по 30 минут свободны
        }
    }
}
