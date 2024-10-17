
namespace HairSalon.Model.Records
{
    public class FakeRepoOfRecords : IRepositoryOfRecords
    {
        List<Record> _records;
        public FakeRepoOfRecords() 
        {
            _records = new List<Record>
            {
                new(){Id = 1, ClientName = "Мария", ClientPhone = "9600000000", SeviceName = "Модельная", DurationOfService = 20, DateTimeForVisit = new DateTime(2025, 01, 15, 10, 0, 0), EmployeeId = 1},
                new(){Id = 2, ClientName = "Елена", ClientPhone = "9600000000", SeviceName = "Каре",  DurationOfService = 20, DateTimeForVisit = new DateTime(2025, 01, 15, 10, 30, 0), EmployeeId = 1},
                new(){Id = 3, ClientName = "Николай", ClientPhone = "9600000000", SeviceName = "Полубокс",  DurationOfService = 20, DateTimeForVisit = new DateTime(2025, 01, 15, 11, 0, 0), EmployeeId = 1}            };
        }
        public int Add(Record record)
        {
            //проверям не занято ли это время
            Record? result = _records.FirstOrDefault(r=> r.DateTimeForVisit == record.DateTimeForVisit);
            if(result == null)
            {
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
    }
}
