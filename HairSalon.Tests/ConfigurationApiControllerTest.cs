using HairSalon.Controllers.Api;
using HairSalon.Model;
using HairSalon.Model.Configuration;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace HairSalon.Tests
{
    public class ConfigurationApiControllerTest
    {
        [Fact]
        public void Get()
        {
            //Arrange
            var mock = new Mock<IRepositoryOfConfiguration>();
            mock.Setup(repo=>repo.GetConfig()).Returns(GetTestConfig());
            ConfigurationApiController controller = new(mock.Object);

            //Act
            JsonResult jsonResult = controller.Get();
            PackageMessage? packageMessage = jsonResult?.Value as PackageMessage;

            //Assert
            Assert.True((packageMessage?.Data as Config)?.MobileAppEnabled);
        }

        [Fact]
        public void Set()
        {
            //Arrange
            var mock = new Mock<IRepositoryOfConfiguration>();
            ConfigurationApiController controller = new(mock.Object);
            Config config = new Config() { MobileAppEnabled = false, PromotionEnabled = false };

            //Act
            JsonResult jsonResult = controller.Set(config);
            PackageMessage? packageMessage = jsonResult?.Value as PackageMessage;

            //Assert
            Assert.True(packageMessage?.Succeed);
        }

        private Config GetTestConfig() 
        {
            return new Config() { MobileAppEnabled = true, PromotionEnabled = true };
        }
    }
}
