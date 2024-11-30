using HairSalon.Controllers.Admin;
using HairSalon.Model.Configuration;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace HairSalon.Tests
{
    public class ConfigurationControllerTest
    {
        [Fact]
        public void IndexResult() 
        {
            //Arrange
            Config config = new()
            { 
                MobileAppEnabled = true, 
                PromotionEnabled = true, 
                RecordEnable = true, 
                NumberOfDaysForRecords = 7, 
                StartTimeOfDaty = new TimeOnly(10,0),
                EndTimeOfDaty=new TimeOnly(18,0),
            };
            var mock = new Mock<IRepositoryOfConfiguration>();
            mock.Setup(r=>r.GetConfig()).Returns(config);
            ConfigurationController configurationController = new(mock.Object);

            //Act
            ViewResult viewResult = configurationController.Index();
            var model = viewResult.Model as Config;

            //Assert
            Assert.NotNull(model);
            Assert.IsType<Config>(model);
            Assert.Equal(7, model?.NumberOfDaysForRecords);
        }

        [Fact]
        public void SetConfigurationResult() 
        {
            //Arrange
            var mock = new Mock<IRepositoryOfConfiguration>();
            ConfigurationController configurationController = new(mock.Object);

            //Act
            RedirectResult redirectResult = configurationController.SetConfiguration(new Config());
            var action = redirectResult.Url;

            //Assert
            Assert.NotNull(action);
            Assert.Equal("~/Admin", action);
        }
    }
}
