namespace HairSalon.Model.Records.Admin
{
    public class RecordsForEmployeeAll
    {
        public string EmployeeName { get; set; }
        public List<RecordsOfDay> RecordsOfDays { get; set; }

        public RecordsForEmployeeAll(string empoyeeName, List<RecordsOfDay> recordsOfDays)
        {
            EmployeeName = empoyeeName;
            RecordsOfDays = recordsOfDays;
        }
    }
}
