﻿namespace HairSalon.Model.Records
{
    public interface IRepositoryOfRecords: IRepository<Record>
    {
        Record? Get(string name);
        List<DateOnly> GetDaysForRecords();
        int AddDayForRecords(DateOnly date);
        bool IsDayForRecords(DateOnly date);


    }
}
