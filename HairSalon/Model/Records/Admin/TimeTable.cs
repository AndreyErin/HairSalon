namespace HairSalon.Model.Records.Admin
{
    public class TimeTable
    {
        public DateOnly Day {  get; set; }
        public List<Record> Records { get; set; }

    }
}
