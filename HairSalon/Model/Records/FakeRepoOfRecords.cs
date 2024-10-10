
namespace HairSalon.Model.Records
{
    public class FakeRepoOfRecords : IRepositoryOfRecords
    {
        List<Record> _records;
        public FakeRepoOfRecords() 
        {
            _records = new List<Record>
            {
                new(){Id = 1, Name = "Мария", SeviceName = "Модельная", DateTimeOfRecord = new DateTime(2025, 01, 15, 10, 0, 0)},
                new(){Id = 2, Name = "Елена", SeviceName = "Каре", DateTimeOfRecord = new DateTime(2025, 01, 15, 10, 30, 0)},
                new(){Id = 3, Name = "Николай", SeviceName = "Полубокс", DateTimeOfRecord = new DateTime(2025, 01, 15, 11, 0, 0)}
            };
        }
        public int Add(Record record)
        {
            //проверям не занято ли это время
            Record? result = _records.FirstOrDefault(r=> r.DateTimeOfRecord == record.DateTimeOfRecord);
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
            return _records.FirstOrDefault(r => r.Name == name);
        }

        public List<Record> GetAll()
        {
            return _records;
        }
    }
}
