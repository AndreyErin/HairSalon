namespace HairSalon.Model.Records
{
    public interface IRepositoryOfRecords: IRepository<Record>
    {
        int Update(Record record);
        List<DateOnly> GetDaysForRecords();
        int AddDayForRecords(DateOnly date);

        int DeleteDayForRecords(DateOnly date);
        bool IsDayForRecords(DateOnly date);
    }
}
