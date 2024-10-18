namespace HairSalon.Model.Records
{
    public interface IRepositoryOfRecords: IRepository<Record>
    {
        Record? Get(string name);
    }
}
