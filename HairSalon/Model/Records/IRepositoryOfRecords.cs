namespace HairSalon.Model.Records
{
    public interface IRepositoryOfRecords
    {
        List<Record> GetAll();
        Record? Get(int id);
        Record? Get(string name);
        int Delete(int id);
        int Add(Record record);
    }
}
