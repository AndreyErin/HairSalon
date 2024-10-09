namespace HairSalon.Model.Services
{
    public class Service
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }
        public int Price { get; set; }
        public TimeSpan TimeOfService { get; set; }

    }
}
