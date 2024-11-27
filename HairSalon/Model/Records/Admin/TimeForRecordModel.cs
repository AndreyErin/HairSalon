using HairSalon.Model.Employees;

namespace HairSalon.Model.Records.Admin
{
    public class TimeForRecordModel
    {
        public bool isEnable {  get; set; } = true;
        public TimeOnly Time {  get; set; }

        public DateOnly Date { get; set; }
        public int EmployeeId { get; set; }
        public Record? Record { get; set; }
    }
}
