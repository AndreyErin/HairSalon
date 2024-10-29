namespace HairSalon.Model.Records
{
    public class FreeTimeForRecords
    {
        public DateOnly Date {  get; set; }
        public List<TimeOnly> Times { get; set; }
    }
}
