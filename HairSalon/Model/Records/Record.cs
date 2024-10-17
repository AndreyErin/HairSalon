namespace HairSalon.Model.Records
{
    public class Record
    {
        public int Id { get; set; }
        public string ClientName { get; set; }
        public string ClientPhone { get; set; }
        public string SeviceName { get; set; }
        public int DurationOfService { get; set; }
        public DateTime DateTimeForVisit { get; set; }        
        public int EmployeeId { get; set; }//id парикмахера, к которму записаны
    }
}
