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
                        record.ServiceName = "";
                        record.ClientPhone = "";
                        record.DurationOfService = 0;

                        int result = _records.Update(record);
                        if (result == -1) { return result; }
                    }

                }
                else
                {
                    int result = _records.Add(new()
                    {
                        ClientName = "ВЫКЛ",
                        DateForVisit = recordModel.Date,
                        TimeForVisit = recordModel.Time,
                        EmployeeId = recordModel.EmployeeId
                    });

                    if (result == -1) { return result; }
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
                    int result = _records.Delete(record.Id);
                    if (result == -1) { return result; }
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
                int result = _records.AddDayForRecords(date.Day);
                //if (result == -1) { return result; }
            }

            //выключаем дни для записи
            foreach (var date in datesOff)
            {
                int result = _records.DeleteDayForRecords(date.Day);
                //if (result == -1) { return result; }
            }

            return 1;
        }
    }
}
