
namespace HairSalon.Model.Records
{
    public class FakeRepoOfRecords : IRepositoryOfRecords
    {
        List<Record> _records;
        List<DateOnly> _daysForRecords;
        public FakeRepoOfRecords() 
        {
            var dt1 = DateTime.Now;
            var dt2 = DateTime.Now.AddDays(1);
            var dt3 = DateTime.Now.AddDays(3);
            var dt4 = DateTime.Now.AddDays(4);



            _records = new()
            {
                new(){Id = 1, ClientName = "Мария", ClientPhone = "9600000000", SeviceName = "Модельная", DurationOfService = 20, DateForVisit = new(dt2.Year, dt2.Month, dt2.Day), TimeForVisit = new ( 10, 0, 0), EmployeeId = 1},
                new(){Id = 2, ClientName = "Томара", ClientPhone = "9600000000", SeviceName = "Модельная", DurationOfService = 30, DateForVisit = new(dt2.Year, dt2.Month, dt2.Day), TimeForVisit = new ( 15, 0, 0), EmployeeId = 1},

                new(){Id = 3, ClientName = "Елена", ClientPhone = "9600000000", SeviceName = "Каре",  DurationOfService = 20, DateForVisit = new(dt2.Year, dt2.Month, dt2.Day), TimeForVisit = new TimeOnly( 10, 30, 0), EmployeeId = 2},
                new(){Id = 4, ClientName = "Николай", ClientPhone = "9600000000", SeviceName = "Полубокс",  DurationOfService = 20, DateForVisit = new(dt4.Year, dt4.Month, dt4.Day), TimeForVisit = new(11, 0, 0), EmployeeId = 1}            
            };

            _daysForRecords = new()
            {
                new(dt1.Year, dt1.Month, dt1.Day),                   
                new(dt2.Year, dt2.Month, dt2.Day),
                new(dt3.Year, dt3.Month, dt3.Day),
                new(dt4.Year, dt4.Month, dt4.Day)
            };
        }
        public int Add(Record record)
        {
            //проверям не занято ли это время(у конкретного работника)
            Record? result = _records.FirstOrDefault(r=> 
                r.DateForVisit == record.DateForVisit &&
                r.TimeForVisit == record.TimeForVisit &&
                r.EmployeeId == record.EmployeeId);
            if(result == null)
            {
                //присваеваем id - самое большое id в списке +1
                int id = _records.Select(r => r.Id).Max();
                record.Id = id + 1;

                _records.Add(record);
                return 1;
            }

            return -1;
        }

        public int Delete(int id)
        {
            Record? result = _records.FirstOrDefault(r => r.Id == id);
            if (result != null)
            {
                _records.Remove(result);
                return 1;
            }

            return -1;
        }

        public Record? Get(int id)
        {
            return _records.FirstOrDefault(r => r.Id == id);
        }

        public Record? Get(string name)
        {
            return _records.FirstOrDefault(r => r.ClientName == name);
        }

        public List<Record> GetAll()
        {
            return _records;
        }

        public int Update(Record record)
        {
            bool result = _records.Remove(_records.First(x=>x.Id == record.Id));
            if (result) 
            {
                _records.Add(record);
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public int AddDayForRecords(DateOnly date)
        {
            bool result = _daysForRecords.Contains(date);
            if (!result)
            {
                _daysForRecords.Add(date);
                return 1;
            }
            return -1;
        }

        public List<DateOnly> GetDaysForRecords()
        {
            return _daysForRecords;
        }

        public bool IsDayForRecords(DateOnly date)
        {
            //доступна ли дата для записи
            return _daysForRecords.Contains(date);
        }

        public int DeleteDayForRecords(DateOnly date)
        {
            bool result = _daysForRecords.Contains(date);
            if (result)
            {
                _daysForRecords.Remove(date);
                return 1;
            }
            return -1;
        }


    }
}
