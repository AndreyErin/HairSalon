using System.IO;
using System.Text.Json;

namespace HairSalon.Model.Configuration
{
    public class JsonRepoOfConfiguration : IRepositoryOfConfiguration
    {
        Config _config;
        public JsonRepoOfConfiguration() 
        {
            using StreamReader jsonFile = File.OpenText(Directory.GetCurrentDirectory() + @"\Model\Configuration\config.json");
            string bufferJson = jsonFile.ReadToEnd();
            _config = JsonSerializer.Deserialize<Config>(bufferJson) ?? new() ;
        }
        public Config GetConfig()
        {
            return _config;
        }

        public void SetConfig(Config config)
        {
            _config = config;
            string bufferJson = JsonSerializer.Serialize(config);
            using StreamWriter streamWriter = File.CreateText(Directory.GetCurrentDirectory() + @"\Model\Configuration\config.json");
            streamWriter.Write(bufferJson);
        }

    }
}
