namespace HairSalon.Model.Records.Admin
{
    public class RecordsForEmployeeAllModel
    {
        public string EmployeeName { get; set; }
        public List<TimeTable> RecordsOfDays { get; set; }

        public RecordsForEmployeeAllModel(string empoyeeName, List<TimeTable> recordsOfDays)
        {
            EmployeeName = empoyeeName;
            RecordsOfDays = recordsOfDays;
        }
    }
}
