namespace HairSalon.Model.Records
{
    public class Record
    {
        public int Id { get; set; }
        public string ClientName { get; set; }
        public string ClientPhone { get; set; }
        public string SeviceName { get; set; }
        public int DurationOfService { get; set; }
        public DateOnly DateForVisit { get; set; }
        public TimeOnly TimeForVisit { get; set; }
        public int EmployeeId { get; set; }//id парикмахера, к которму записаны
    }
}
