using HairSalon.Model.Employees;

namespace HairSalon.Model.Records.Admin
{
    //Класс для сортировки имеющихся записей по:
    //сотруднику-дате-времени
    public class RecordsForEmployeeService
    {
        private IRepositoryOfRecords _records;
        private IRepositoryOfEmployees _employees;
        public RecordsForEmployeeService(IRepositoryOfRecords records, IRepositoryOfEmployees employees)
        {
            _records = records;
            _employees = employees;
        }

        public List<RecordsForEmployee> GetRecords() 
        {
            return Sort().ToList();
        }

        public IEnumerable<RecordsForEmployee> Sort()
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
                    recordsOfDays.Add(new(item.Key, item.OrderBy(x=>x.TimeForVisit).ToList()));
                }


                if (recordsOfDays.Count > 0)
                {
                    yield return new RecordsForEmployee(employee.Name, recordsOfDays);
                }
                
            }
        }
    }
}
