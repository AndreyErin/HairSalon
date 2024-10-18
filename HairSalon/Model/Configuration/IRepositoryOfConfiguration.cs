namespace HairSalon.Model.Configuration
{
    public interface IRepositoryOfConfiguration
    {
        Config GetConfig();
        void SetConfig(Config config);
    }
}
