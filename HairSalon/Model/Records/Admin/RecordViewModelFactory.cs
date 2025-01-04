using HairSalon.Model.Configuration;
using HairSalon.Model.Employees;

namespace HairSalon.Model.Records.Admin
{
    //класс создания моделей для представлений
    public class RecordViewModelFactory
    {
        private IRepositoryOfRecords _records;
        private IRepositoryOfEmployees _employees;
        private IRepositoryOfConfiguration _config;
        public RecordViewModelFactory(IRepositoryOfRecords records,
            IRepositoryOfEmployees employees,
            IRepositoryOfConfiguration configuration)
        {
            _records = records;
            _employees = employees;
            _config = configuration;
        }

        public WorkDatesModel[] CreateWorkDatesModelArray() 
        {
            DateOnly startDate = new(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            List<WorkDatesModel> model = new();

            for (int i = 0; i < 31; i++)
            {
                bool enable = _records.GetDaysForRecords().Contains(startDate);
                model.Add(new() { Day =  startDate, IsEnable = enable });
                startDate = startDate.AddDays(1);
            }

            return model.ToArray(); 
        }

        public List<RecordsForEmployeeAllModel> CreateRecordsForEmployeeAllDaysModelList()
        {
            return Sort().ToList();
        }

        //Сортировка имеющихся записей по:
        //сотруднику-дате-времени
        private IEnumerable<RecordsForEmployeeAllModel> Sort()
        {
            DateOnly toDay = new(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            foreach (var employee in _employees.GetAll())
            {
                //выбираем только актуальные записи, т.е. от сегодня и позже
                //выбираем все записи для конкретного сотрудника
                List<Record> empRecords = _records.GetAll().Where(x => x.EmployeeId == employee.Id && x.ClientName != "ВЫКЛ" && x.DateForVisit >= toDay).ToList();
                //разбиваем эти записи по дням
                var grups = empRecords.GroupBy(x => x.DateForVisit);

                List<TimeTable> recordsOfDays = new();

                foreach (var item in grups)
                {
                    recordsOfDays.Add(new() { Day = item.Key, Records = item.OrderBy(x => x.TimeForVisit).ToList() });
                }


                if (recordsOfDays.Count > 0)
                {
                    yield return new RecordsForEmployeeAllModel(employee.Name, recordsOfDays);
                }

            }
        }
    
        public List<RecordsForEmployeeOfDayModel> CreateRecordsForEmployeeOfDayModelList(DateOnly day)
        {
            List<RecordsForEmployeeOfDayModel> model = new();

            foreach (var emp in _employees.GetAll())
            {
                var sub = _records.GetAll().Where(x => x.EmployeeId == emp.Id && x.DateForVisit == day);

                var starTime = _config.GetConfig().StartTimeOfDay;
                var endTime = _config.GetConfig().EndTimeOfDay;                
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

                TimeTable recordsOfDay = new() { Day = day, Records = recordList };
                RecordsForEmployeeOfDayModel recordsForEmployeeOfDay = new() { Employee = emp, RecordsOfDay = recordsOfDay };

                model.Add(recordsForEmployeeOfDay);
            }

            return model;
        }

        public TimeForRecordModel[] CreateTimeForRecordModelArray(DateOnly dateDay, int employeeId)
        {
            Employee? emp = _employees.Get(employeeId);

            var subset = _records.GetAll().Where(x=>x.DateForVisit == dateDay && x.EmployeeId == employeeId );
          
            var starTime = _config.GetConfig().StartTimeOfDay;
            var endTime = _config.GetConfig().EndTimeOfDay;
            List<TimeForRecordModel> result = new();

            while (starTime != endTime)
            {
                Record? record = subset.FirstOrDefault(x => x.TimeForVisit == starTime);

                if (record != null && emp != null)
                {
                    if (record?.ClientName != "ВЫКЛ") 
                    {
                        result.Add(new() { Time = starTime, Record = record, EmployeeId = emp.Id, Date = dateDay });
                    }
                    else
                    {
                        result.Add(new() { Time = starTime, Record = record, EmployeeId = emp.Id, Date = dateDay, isEnable = false});
                    }                   
                }
                else
                {
                    result.Add(new() { Time = starTime, EmployeeId = emp.Id, Date = dateDay });
                }

                starTime = starTime.AddMinutes(30);
            }

            return result.ToArray();
        }


    }
}
