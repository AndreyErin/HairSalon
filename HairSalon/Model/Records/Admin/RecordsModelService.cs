using HairSalon.Model.Configuration;
using HairSalon.Model.Employees;

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
                model.Add(new(startDate, enable));
                startDate = startDate.AddDays(1);
            }

            return model; 
        }

        public int SetDaysForRecords(List<DateOnly> dates) 
        {
            //foreach (var day in dates)
            //{
            //    int result = _records.AddDayForRecords(day);
            //    if(result == -1)
            //    {
            //        return -1;
            //    }
            //}       
            return 1; 
        }

    }
}
