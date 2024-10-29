﻿
namespace HairSalon.Model.Records
{
    public class FakeRepoOfRecords : IRepositoryOfRecords
    {
        List<Record> _records;
        List<DateOnly> _daysForRecords;
        public FakeRepoOfRecords() 
        {
            _records = new()
            {
                new(){Id = 1, ClientName = "Мария", ClientPhone = "9600000000", SeviceName = "Модельная", DurationOfService = 20, DateForVisit = new(2024, 11 , 1), TimeForVisit = new ( 10, 0, 0), EmployeeId = 1},
                new(){Id = 2, ClientName = "Елена", ClientPhone = "9600000000", SeviceName = "Каре",  DurationOfService = 20, DateForVisit = new(2024, 11 , 1), TimeForVisit = new TimeOnly( 10, 30, 0), EmployeeId = 1},
                new(){Id = 3, ClientName = "Николай", ClientPhone = "9600000000", SeviceName = "Полубокс",  DurationOfService = 20, DateForVisit = new(2024, 11 , 3), TimeForVisit = new(11, 0, 0), EmployeeId = 1}            
            };

            _daysForRecords = new()
            {
                new(2024, 11 , 1),
                new(2024, 11 , 3),
                new(2024, 11 , 5)
            };
        }
        public int Add(Record record)
        {
            //проверям не занято ли это время
            Record? result = _records.FirstOrDefault(r=> r.DateForVisit == record.DateForVisit && r.TimeForVisit == record.TimeForVisit);
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
    }
}
