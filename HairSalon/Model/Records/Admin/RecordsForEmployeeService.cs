using HairSalon.Model.Employees;

namespace HairSalon.Model.Records.Admin
{
    public class RecordsForEmployeeService
    {
        private List<Record> _records;
        private List<Employee> _employees;
        public RecordsForEmployeeService(List<Record> records, List<Employee> employees)
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
            foreach (var employee in _employees)
            {
                //выбираем все записи для конкретного сотрудника
                List<Record> empRecords = _records.Where(x => x.EmployeeId == employee.Id).ToList();
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
