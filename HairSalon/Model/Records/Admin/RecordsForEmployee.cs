namespace HairSalon.Model.Records.Admin
{
    public class RecordsForEmployee
    {
        public string EmployeeName { get; set; }
        public List<RecordsOfDay> RecordsOfDays { get; set; }

        public RecordsForEmployee(string empoyeeName, List<RecordsOfDay> recordsOfDays)
        {
            EmployeeName = empoyeeName;
            RecordsOfDays = recordsOfDays;
        }
    }
}
