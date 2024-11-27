using HairSalon.Model.Configuration;
using HairSalon.Model.Employees;
using System.Collections.Generic;

namespace HairSalon.Model.Records.Admin
{
    public class RecordsModelService
    {
        private IRepositoryOfRecords _records;
        private IRepositoryOfEmployees _employees;
        private IRepositoryOfConfiguration _config;
        public RecordsModelService(IRepositoryOfRecords records,
            IRepositoryOfEmployees employees,
            IRepositoryOfConfiguration configuration)
        {
            _records = records;
            _employees = employees;
            _config = configuration;
        }

        public List<DayForRecordsModel> GetDaysForRecords() 
        {
            DateOnly startDate = new(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            List<DayForRecordsModel> model = new();

            for (int i = 0; i < 31; i++)
            {
                bool enable = _records.GetDaysForRecords().Contains(startDate);
                model.Add(new() { Day =  startDate, IsEnable = enable });
                startDate = startDate.AddDays(1);
            }

            return model; 
        }

        public int SetDaysForRecords(List<DayForRecordsModel> dates) 
        {
            var datesOn = dates.Where(x => x.IsEnable == true);
            var datesOff = dates.Where(x => x.IsEnable == false);

            //включаем дни для записи
            foreach (var date in datesOn)
            {
                _records.AddDayForRecords(date.Day);
            }

            //выключаем дни для записи
            foreach (var date in datesOff)
            {
                _records.DeleteDayForRecords(date.Day);
            }

            return 1; 
        }

        public List<RecordsForEmployeeAll> GetAllRecordsForEmployees()
        {
            return Sort().ToList();
        }

        //Сортировка имеющихся записей по:
        //сотруднику-дате-времени
        private IEnumerable<RecordsForEmployeeAll> Sort()
        {
            foreach (var employee in _employees.GetAll())
            {
                //выбираем все записи для конкретного сотрудника
                List<Record> empRecords = _records.GetAll().Where(x => x.EmployeeId == employee.Id).ToList();
                //разбиваем эти записи по дням
                var grups = empRecords.GroupBy(x => x.DateForVisit);

                List<RecordsOfDay> recordsOfDays = new();

                foreach (var item in grups)
                {
                    recordsOfDays.Add(new() { Day = item.Key, Records = item.OrderBy(x => x.TimeForVisit).ToList() });
                }


                if (recordsOfDays.Count > 0)
                {
                    yield return new RecordsForEmployeeAll(employee.Name, recordsOfDays);
                }

            }
        }
    
        public List<RecordsForEmployeeOfDay> GetRecordsOfDayForEmpoyees(DateOnly day)
        {
            List<RecordsForEmployeeOfDay> model = new();

            foreach (var emp in _employees.GetAll())
            {
                var sub = _records.GetAll().Where(x => x.EmployeeId == emp.Id && x.DateForVisit == day);

                var starTime = _config.GetConfig().StartTimeOfDaty;
                var endTime = _config.GetConfig().EndTimeOfDaty;                
                List<Record> recordList = new();

                while (starTime != endTime)
                {
                    Record? record = sub.FirstOrDefault(x=>x.TimeForVisit == starTime);

                    if (record != null)
                    {
                        recordList.Add(record);
                    }
                    else 
                    {
                        recordList.Add(new() { TimeForVisit = starTime});
                    }

                    starTime = starTime.AddMinutes(30);
                }

                RecordsOfDay recordsOfDay = new() { Day = day, Records = recordList };
                RecordsForEmployeeOfDay recordsForEmployeeOfDay = new() { EmployeeName = emp.Name, RecordsOfDay = recordsOfDay };

                model.Add(recordsForEmployeeOfDay);
            }

            return model;
        }
    }
}
