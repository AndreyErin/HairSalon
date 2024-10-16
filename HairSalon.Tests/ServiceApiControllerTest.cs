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
            var mock = new Mock<IRepositoryOfServices<Service>>();
            mock.Setup(repo => repo.Get(id)).Returns(GetAllService().FirstOrDefault(s=>s.Id == id));
            //id2 - недостижимый индекс
            mock.Setup(repo => repo.Get(id2)).Returns(GetAllService().FirstOrDefault(s => s.Id == id2));
            ServicesApiController servicesApiController = new(mock.Object);

            //Act
            JsonResult jsonResult1 = servicesApiController.Get(id);
            JsonResult jsonResult2 = servicesApiController.Get(id2);
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
            Service service1 = new()
            {
                Id = 55,
                Picture = "",
                Name = "Ёлочка",
                Description = "Под ёлку",
                Price = 555,
                TimeOfService = new(0, 20, 0)
            };
            Service service2 = new()
            {
                Id = 33,
                Picture = "",
                Name = "Пикси",
                Description = "Под Пикси)",
                Price = 333,
                TimeOfService = new(0, 25, 0)
            };
            //успех
            var mock = new Mock<IRepositoryOfServices<Service>>();
            mock.Setup(repo => repo.Add(service1)).Returns(1);          
            //неудача
            mock.Setup(repo => repo.Add(service2)).Returns(0);
            ServicesApiController servicesApiController = new(mock.Object);

            //Act
            JsonResult jsonResult1 = servicesApiController.Add(service1);
            JsonResult jsonResult2 = servicesApiController.Add(service2);
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
            var mock = new Mock<IRepositoryOfServices<Service>>();
            mock.Setup(repo => repo.Delete(id)).Returns(1);
            //id2 - недостижимый индекс
            mock.Setup(repo => repo.Delete(id2)).Returns(0);
            ServicesApiController servicesApiController = new(mock.Object);

            //Act
            JsonResult jsonResult1 = servicesApiController.Delete(id);
            JsonResult jsonResult2 = servicesApiController.Delete(id2);
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
            Service service1 = new()
            {
                Id = 55,
                Picture = "",
                Name = "Ёлочка",
                Description = "Под ёлку",
                Price = 555,
                TimeOfService = new(0, 20, 0)
            };
            Service service2 = new()
            {
                Id = 33,
                Picture = "",
                Name = "Пикси",
                Description = "Под Пикси)",
                Price = 333,
                TimeOfService = new(0, 25, 0)
            };
            var mock = new Mock<IRepositoryOfServices<Service>>();
            mock.Setup(repo => repo.Update(service1)).Returns(1);
            //id2 - недостижимый индекс
            mock.Setup(repo => repo.Update(service2)).Returns(0);
            ServicesApiController servicesApiController = new(mock.Object);

            //Act
            JsonResult jsonResult1 = servicesApiController.Update(service1);
            JsonResult jsonResult2 = servicesApiController.Update(service2);
            var result1 = (jsonResult1.Value as PackageMessage)?.Succeed;
            var result2 = (jsonResult2.Value as PackageMessage)?.Succeed;

            //Assert
            Assert.Equal(true, result1);
            Assert.Equal(false, result2);
        }
    }
}
