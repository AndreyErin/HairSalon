using HairSalon.Controllers;
using HairSalon.Model;
using HairSalon.Model.Configuration;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace HairSalon.Tests
{
    public class ConfigurationApiControllerTests
    {
        [Fact]
        public void Get()
        {
            //Arrange
            var mock = new Mock<IRepositoryOfConfiguration>();
            mock.Setup(repo=>repo.GetConfig()).Returns(GetTestConfig());
            ConfigurationApiController controller = new(mock.Object);

            //Act
            JsonResult result = controller.Get();
            PackageMessage? packageMessage = result?.Value as PackageMessage;

            //Assert
            Assert.Equal(true, (packageMessage?.Data as Config)?.MobileAppEnabled);
        }

        [Fact]
        public void Set()
        {
            //Arrange
            var mock = new Mock<IRepositoryOfConfiguration>();
            ConfigurationApiController controller = new(mock.Object);
            Config config = new Config() { MobileAppEnabled = false, PromotionEnabled = false };

            //Act
            JsonResult result = controller.Set(config);
            PackageMessage? packageMessage = result?.Value as PackageMessage;

            //Assert
            Assert.Equal(true, packageMessage?.Succeed);
        }

        private Config GetTestConfig() 
        {
            return new Config() { MobileAppEnabled = true, PromotionEnabled = true };
        }
    }
}
