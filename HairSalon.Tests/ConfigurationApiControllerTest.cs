using HairSalon.Controllers.Api.v1;
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
            var result = controller.Get();

            //Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
            Assert.IsType<Config>((result as OkObjectResult)?.Value);
        }

        [Fact]
        public void Set()
        {
            //Arrange
            var mock = new Mock<IRepositoryOfConfiguration>();
            mock.Setup(repo => repo.GetConfig()).Returns(GetTestConfig());
            ConfigurationApiController controller = new(mock.Object);
            Config config1 = new() { 
                MobileAppEnabled = false,
                PromotionEnabled = false,
                RecordEnabled = false,
                NumberOfDaysForRecords = 2,
                StartTimeOfDay = new(8, 0),
                EndTimeOfDay = new(16, 0)};
            Config config2 = new() { MobileAppEnabled = false, PromotionEnabled = false };

            //Act
            var result1 = controller.Set(config1);
            var result2 = controller.Set(config2);

            //Assert
            Assert.NotNull(result1);
            Assert.IsType<OkObjectResult>(result1);
            Assert.IsType<Config>((result1 as OkObjectResult)?.Value);

            Assert.NotNull(result2);
            Assert.IsType<UnprocessableEntityObjectResult>(result2);
            Assert.IsType<string>((result2 as UnprocessableEntityObjectResult)?.Value);
        }

        private Config GetTestConfig() 
        {
            return new()
            {
                MobileAppEnabled = false,
                PromotionEnabled = false,
                RecordEnabled = false,
                NumberOfDaysForRecords = 2,
                StartTimeOfDay = new(8, 0),
                EndTimeOfDay = new(16, 0)
            };
        }
    }
}
