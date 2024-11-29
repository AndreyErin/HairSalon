namespace HairSalon.Model.Records.Admin
{
    public class RecordsForEmployeeAll
    {
        public string EmployeeName { get; set; }
        public List<TimeTable> RecordsOfDays { get; set; }

        public RecordsForEmployeeAll(string empoyeeName, List<TimeTable> recordsOfDays)
        {
            EmployeeName = empoyeeName;
            RecordsOfDays = recordsOfDays;
        }
    }
}
