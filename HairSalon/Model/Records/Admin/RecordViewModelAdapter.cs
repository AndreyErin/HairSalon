namespace HairSalon.Model.Records.Admin
{
    //класс для обработки данных полученных из представлений

    public class RecordViewModelAdapter
    {
        private IRepositoryOfRecords _records;

        public RecordViewModelAdapter(IRepositoryOfRecords records)
        {
            _records = records;
        }

        public int SetTimeTable(TimeForRecordModel[] recordModels)
        {
            //выключаем время
            List<TimeForRecordModel> disabledList = recordModels.ToList().Where(x => x.isEnable == false).ToList();

            foreach (var recordModel in disabledList)
            {
                var record = _records.GetAll().FirstOrDefault(x =>
                    x.DateForVisit == recordModel.Date &&
                    x.TimeForVisit == recordModel.Time &&
                    x.EmployeeId == recordModel.EmployeeId
                    );

                if (record != null)
                {
                    if (record.ClientName != "ВЫКЛ")
                    {
                        record.ClientName = "ВЫКЛ";
                        record.SeviceName = "";
                        record.ClientPhone = "";
                        record.DurationOfService = 0;
                        _records.Update(record);
                    }

                }
                else
                {
                    _records.Add(new()
                    {
                        ClientName = "ВЫКЛ",
                        DateForVisit = recordModel.Date,
                        TimeForVisit = recordModel.Time,
                        EmployeeId = recordModel.EmployeeId
                    });
                }
            }

            List<TimeForRecordModel> enabledList = recordModels.ToList().Where(x => x.isEnable == true).ToList();

            foreach (var recordModel in enabledList)
            {
                var record = _records.GetAll().FirstOrDefault(x =>
                    x.DateForVisit == recordModel.Date &&
                    x.TimeForVisit == recordModel.Time &&
                    x.EmployeeId == recordModel.EmployeeId &&
                    x.ClientName == "ВЫКЛ"
                    );

                if (record != null)
                {
                    _records.Delete(record.Id);
                }
            }

            return 1;
        }

        public int SetWorkDates(WorkDatesModel[] dates)
        {
            var datesOn = dates.ToList().Where(x => x.IsEnable == true);
            var datesOff = dates.ToList().Where(x => x.IsEnable == false);

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
    }
}
