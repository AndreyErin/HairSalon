using HairSalon.Controllers;
using HairSalon.Model;
using HairSalon.Model.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace HairSalon.Tests
{
    public class ServiceApiControllerTest
    {
        [Fact]
        public void GetAllResultData()
        {
            //Arrange
            var mock = new Mock<IRepositoryOfServices<Service>>();
            mock.Setup(repo => repo.GetAll()).Returns(GetAllService); ;
            ServicesApiController servicesApiController = new(mock.Object);
            PackageMessage? packageMessage = servicesApiController.GetAll().Value as PackageMessage;

            //Act
            List<Service>? services = packageMessage?.Data as List<Service>;
            var resultCount = services?.Count;

            //Assert
            Assert.Equal(5, resultCount);
            Assert.Equal("Котовский", services?.FirstOrDefault(s=>s.Id == 4)?.Name);          
        }

        private List<Service> GetAllService()
        {
            return new() {
                new Service { Id = 1, Name = "Полубокс", Price = 100, TimeOfService = new TimeSpan(0,20,0), Description = "Под полубоксера" },
                new Service { Id = 2, Name = "Тенис", Price = 200, TimeOfService = new TimeSpan(0,25,0), Description = "Под тенисиста" },
                new Service { Id = 3, Name = "Модельная", Price = 300, TimeOfService = new TimeSpan(0,30,0), Description = "Под модель" },
                new Service { Id = 4, Name = "Котовский", Price = 400, TimeOfService = new TimeSpan(0,15,0), Description = "Под Котовского" },
                new Service { Id = 5, Name = "Гаршок", Price = 500, TimeOfService = new TimeSpan(0,10,0), Description = "Под горшок" },
            };
        }

        [Fact]
        public void GetResult()
        {
            //Arrange
            int id = 1, id2 = 20;    
            var mock1 = new Mock<IRepositoryOfServices<Service>>();
            mock1.Setup(repo => repo.Get(id)).Returns(GetAllService().FirstOrDefault(s=>s.Id == id));
            ServicesApiController servicesApiController1 = new(mock1.Object);
            var mock2 = new Mock<IRepositoryOfServices<Service>>();
            //id2 - недостижимый индекс
            mock2.Setup(repo => repo.Get(id2)).Returns(GetAllService().FirstOrDefault(s => s.Id == id2));
            ServicesApiController servicesApiController2 = new(mock2.Object);

            //Act
            JsonResult jsonResult1 = servicesApiController1.Get(id);
            JsonResult jsonResult2 = servicesApiController1.Get(id2);
            var result1 = (jsonResult1.Value as PackageMessage)?.Succeed;
            var result2 = (jsonResult2.Value as PackageMessage)?.Succeed;

            //Assert
            Assert.Equal(true, result1);
            Assert.Equal(false, result2);
        }


        [Fact]
        public void AddResult()
        {
            //Arrange
            Service service = new()
            {
                Id = 55,
                Picture = "",
                Name = "Ёлочка",
                Description = "Под ёлку",
                Price = 555,
                TimeOfService = new(0, 20, 0)
            };
            //успех
            var mock1 = new Mock<IRepositoryOfServices<Service>>();
            mock1.Setup(repo => repo.Add(service)).Returns(1);
            ServicesApiController servicesApiController1 = new(mock1.Object);
            //неудача
            var mock2 = new Mock<IRepositoryOfServices<Service>>();
            mock2.Setup(repo => repo.Add(service)).Returns(0);
            ServicesApiController servicesApiController2 = new(mock2.Object);

            //Act
            JsonResult jsonResult1 = servicesApiController1.Add(service);
            JsonResult jsonResult2 = servicesApiController2.Add(service);
            var result = (jsonResult1.Value as PackageMessage)?.Succeed;
            var result2 = (jsonResult2.Value as PackageMessage)?.Succeed;

            //Assert           
            Assert.Equal(true, result);
            Assert.Equal(false, result2);
        }

        [Fact]
        public void DeleteResult()
        {
            //Arrange
            int id = 1, id2 = 20;
            var mock1 = new Mock<IRepositoryOfServices<Service>>();
            mock1.Setup(repo => repo.Delete(id)).Returns(1);
            ServicesApiController servicesApiController1 = new(mock1.Object);
            var mock2 = new Mock<IRepositoryOfServices<Service>>();
            //id2 - недостижимый индекс
            mock2.Setup(repo => repo.Delete(id2)).Returns(0);
            ServicesApiController servicesApiController2 = new(mock2.Object);

            //Act
            JsonResult jsonResult1 = servicesApiController1.Delete(id);
            JsonResult jsonResult2 = servicesApiController2.Delete(id2);
            var result1 = (jsonResult1.Value as PackageMessage)?.Succeed;
            var result2 = (jsonResult2.Value as PackageMessage)?.Succeed;

            //Assert
            Assert.Equal(true, result1);
            Assert.Equal(false, result2);
        }

        [Fact]
        public void UpdateResult()
        {
            //Arrange
            Service service = new()
            {
                Id = 55,
                Picture = "",
                Name = "Ёлочка",
                Description = "Под ёлку",
                Price = 555,
                TimeOfService = new(0, 20, 0)
            };
            var mock1 = new Mock<IRepositoryOfServices<Service>>();
            mock1.Setup(repo => repo.Update(service)).Returns(1);
            ServicesApiController servicesApiController1 = new(mock1.Object);
            var mock2 = new Mock<IRepositoryOfServices<Service>>();
            //id2 - недостижимый индекс
            mock2.Setup(repo => repo.Update(service)).Returns(0);
            ServicesApiController servicesApiController2 = new(mock2.Object);

            //Act
            JsonResult jsonResult1 = servicesApiController1.Update(service);
            JsonResult jsonResult2 = servicesApiController2.Update(service);
            var result1 = (jsonResult1.Value as PackageMessage)?.Succeed;
            var result2 = (jsonResult2.Value as PackageMessage)?.Succeed;

            //Assert
            Assert.Equal(true, result1);
            Assert.Equal(false, result2);
        }
    }
}
