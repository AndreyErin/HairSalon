using HairSalon.Controllers.Api.v1;
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
            var mockServices1 = new Mock<IRepositoryOfServices>();
            mockServices1.Setup(repo => repo.GetAll()).Returns(GetAllService); ;
            ServicesApiController servicesApiController1 = new(mockServices1.Object);
            var mockServices2 = new Mock<IRepositoryOfServices>();
            mockServices2.Setup(repo => repo.GetAll()).Returns(new List<Service>()); ;
            ServicesApiController servicesApiController2 = new(mockServices2.Object);

            //Act
            var resultOk = servicesApiController1.GetAll();
            var resultNotFound = servicesApiController2.GetAll();

            //Assert
            Assert.NotNull(resultOk);
            Assert.IsType<OkObjectResult>(resultOk);
            Assert.IsType<List<Service>>((resultOk as ObjectResult)?.Value);

            Assert.NotNull(resultNotFound);
            Assert.IsType<NotFoundObjectResult>(resultNotFound);
            Assert.IsType<string>((resultNotFound as ObjectResult)?.Value);
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
            int id1 = 1, id2 = 20, id3 = 0;    
            var mock = new Mock<IRepositoryOfServices>();
            mock.Setup(repo => repo.Get(id1)).Returns(GetAllService().FirstOrDefault(s=>s.Id == id1));
            //id2 - недостижимый индекс
            mock.Setup(repo => repo.Get(id2)).Returns(GetAllService().FirstOrDefault(s => s.Id == id2));
            ServicesApiController servicesApiController = new(mock.Object);

            //Act
            var resultOk = servicesApiController.Get(id1);
            var resultNotFound = servicesApiController.Get(id2);
            var resultUnprocessableEntity = servicesApiController.Get(id3);

            //Assert
            Assert.NotNull(resultOk);
            Assert.IsType<OkObjectResult>(resultOk);
            Assert.IsType<Service>((resultOk as ObjectResult)?.Value);

            Assert.NotNull(resultNotFound);
            Assert.IsType<NotFoundObjectResult>(resultNotFound);
            Assert.IsType<string>((resultNotFound as ObjectResult)?.Value);

            Assert.NotNull(resultUnprocessableEntity);
            Assert.IsType<UnprocessableEntityObjectResult>(resultUnprocessableEntity);
            Assert.IsType<string>((resultUnprocessableEntity as ObjectResult)?.Value);
        }


        [Fact]
        public void AddResult()
        {
            //Arrange
            Service service1 = new()
            {
                Id =55,
                Picture = "/picture1",
                Name = "Ёлочка",
                Description = "Под ёлку",
                Price = 555,
                TimeOfService = new(0, 20, 0)
            };
            Service service2 = new()
            {
                Id = 33,
                Picture = "/picture2",
                Name = "Пикси",
                Description = "Под Пикси)",
                Price = 333,
                TimeOfService = new(0, 25, 0)
            };
            //успех
            var mock = new Mock<IRepositoryOfServices>();
            mock.Setup(repo => repo.Add(service1)).Returns(1);          
            //неудача
            mock.Setup(repo => repo.Add(service2)).Returns(0);
            ServicesApiController servicesApiController = new(mock.Object);

            //Act
            var resultOk = servicesApiController.Add(service1);
            var resultConflict = servicesApiController.Add(service2);
            var resultUnprocessableEntity = servicesApiController.Add(new Service());

            //Assert
            Assert.NotNull(resultOk);
            Assert.IsType<OkObjectResult>(resultOk);
            Assert.IsType<Service>((resultOk as ObjectResult)?.Value);

            Assert.NotNull(resultConflict);
            Assert.IsType<ConflictObjectResult>(resultConflict);
            Assert.IsType<string>((resultConflict as ObjectResult)?.Value);

            Assert.NotNull(resultUnprocessableEntity);
            Assert.IsType<UnprocessableEntityObjectResult>(resultUnprocessableEntity);
            Assert.IsType<string>((resultUnprocessableEntity as ObjectResult)?.Value);
        }

        [Fact]
        public void DeleteResult()
        {
            //Arrange
            int id1 = 1, id2 = 20, id3 = 0;
            var mock = new Mock<IRepositoryOfServices>();
            mock.Setup(repo => repo.Delete(id1)).Returns(1);
            //id2 - недостижимый индекс
            mock.Setup(repo => repo.Delete(id2)).Returns(0);
            ServicesApiController servicesApiController = new(mock.Object);

            //Act
            var resultOk = servicesApiController.Delete(id1);
            var resultNotFound = servicesApiController.Delete(id2);
            var resultUnprocessableEntity = servicesApiController.Delete(id3);

            //Assert
            Assert.NotNull(resultOk);
            Assert.IsType<OkResult>(resultOk);

            Assert.NotNull(resultNotFound);
            Assert.IsType<NotFoundObjectResult>(resultNotFound);
            Assert.IsType<string>((resultNotFound as ObjectResult)?.Value);

            Assert.NotNull(resultUnprocessableEntity);
            Assert.IsType<UnprocessableEntityObjectResult>(resultUnprocessableEntity);
            Assert.IsType<string>((resultUnprocessableEntity as ObjectResult)?.Value);
        }

        [Fact]
        public void UpdateResult()
        {
            //Arrange
            Service service1 = new()
            {
                Id = 55,
                Picture = "/picture1",
                Name = "Ёлочка",
                Description = "Под ёлку",
                Price = 555,
                TimeOfService = new(0, 20, 0)
            };
            Service service2 = new()
            {
                Id = 33,
                Picture = "/picture2",
                Name = "Пикси",
                Description = "Под Пикси)",
                Price = 333,
                TimeOfService = new(0, 25, 0)
            };
            var mock = new Mock<IRepositoryOfServices>();
            mock.Setup(repo => repo.Update(service1)).Returns(1);
            //id2 - недостижимый индекс
            mock.Setup(repo => repo.Update(service2)).Returns(0);
            ServicesApiController servicesApiController = new(mock.Object);

            //Act
            var resultOk = servicesApiController.Update(service1);
            var resultNotFound = servicesApiController.Update(service2);
            var resultUnprocessableEntity = servicesApiController.Update(new Service());

            //Assert
            Assert.NotNull(resultOk);
            Assert.IsType<OkObjectResult>(resultOk);
            Assert.IsType<Service>((resultOk as ObjectResult)?.Value);

            Assert.NotNull(resultNotFound);
            Assert.IsType<NotFoundObjectResult>(resultNotFound);
            Assert.IsType<string>((resultNotFound as ObjectResult)?.Value);

            Assert.NotNull(resultUnprocessableEntity);
            Assert.IsType<UnprocessableEntityObjectResult>(resultUnprocessableEntity);
            Assert.IsType<string>((resultUnprocessableEntity as ObjectResult)?.Value);
        }
    }
}
