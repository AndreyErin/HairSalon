using HairSalon.Model.Employees;

namespace HairSalon.Model.Records.Admin
{
    public class RecordsForEmployeeOfDay
    {
        public Employee Employee { get; set; }
        public RecordsOfDay RecordsOfDay { get; set; }
    }
}
