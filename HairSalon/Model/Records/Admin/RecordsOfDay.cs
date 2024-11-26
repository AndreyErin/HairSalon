namespace HairSalon.Model.Records.Admin
{
    public class RecordsOfDay
    {
        public DateOnly Day {  get; set; }
        public List<Record> Records { get; set; }

    }
}
