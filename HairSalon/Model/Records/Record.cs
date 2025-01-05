namespace HairSalon.Model.Records
{
    public class Record
    {
        public int Id { get; set; }
        public string ClientName { get; set; } = "";
        public string ClientPhone { get; set; } = "";
        public string ServiceName { get; set; } = "";
        public int DurationOfService { get; set; }
        public DateOnly DateForVisit { get; set; }
        public TimeOnly TimeForVisit { get; set; }
        public int EmployeeId { get; set; }//id парикмахера, к которму записаны


        public bool IsValid()
        {
            if (ClientName.Trim() != "" &&
                ClientPhone.Trim() != "" &&
                ServiceName.Trim() != "" &&
                DurationOfService > 0 &&
                DateForVisit > DateOnly.MinValue &&
                TimeForVisit > TimeOnly.MinValue &&
                EmployeeId > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
