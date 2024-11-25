namespace HairSalon.Model.Records.Admin
{
    public class DayForRecordsModel
    {
        public DateOnly Day {  get; set; }
        public bool IsEnable {  get; set; }

        public DayForRecordsModel(DateOnly day, bool isEnable)
        {
            Day = day;
            IsEnable = isEnable;
        }
    }
}
