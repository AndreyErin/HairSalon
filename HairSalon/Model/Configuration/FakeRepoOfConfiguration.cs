namespace HairSalon.Model.Configuration
{
    public class FakeRepoOfConfiguration : IRepositoryOfConfiguration
    {
        Config _config;
        public FakeRepoOfConfiguration() 
        { 
            _config = new() { MobileAppEnabled = true, PromotionEnabled = true };
        }

        public Config GetConfig()
        {
            return _config;
        }

        public void SetConfig(Config config)
        {
            _config = config;
        }

    }
}
