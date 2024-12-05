using HairSalon.Controllers.Admin;
using HairSalon.Model.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace HairSalon.Tests
{
    public class ServicesControllerTest
    {
        [Fact]
        public void IndexResult()
        {
            //Arrange
            var mockServices = new Mock<IRepositoryOfServices>();
            mockServices.Setup(x=>x.GetAll()).Returns(RepositoryOfService_GetAll);
            ServicesController servicesController = new(mockServices.Object);

            //Act
            var result = servicesController.Index();

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
            Assert.IsType<List<Service>>(result.Model);
            Assert.Equal(5, (result.Model as List<Service>)?.Count);
        }

        [Fact]
        public void Add_GetResult()
        {
            //Arrange
            var mockServices = new Mock<IRepositoryOfServices>();
            ServicesController servicesController = new(mockServices.Object);

            //Act
            var result = servicesController.Add();

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
            Assert.IsType<Service>(result.Model);
            Assert.Equal("AddOrEdit", result.ViewName);
            Assert.Equal("Добавить", result.ViewData["Title"]);
        }

        [Fact]
        public void Add_PostResult()
        {
            //Arrange
            var mockServices = new Mock<IRepositoryOfServices>();
            mockServices.Setup(x=>x.Add(It.IsAny<Service>())).Returns(1);
            ServicesController servicesController = new(mockServices.Object);

            //Act
            var result = servicesController.Add(new());

            //Assert
            Assert.NotNull(result);
            Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", result.ActionName);
        }


        [Fact]
        public void Edit_GetResult()
        {
            //Arrange
            var mockServices = new Mock<IRepositoryOfServices>();
            mockServices.Setup(x => x.Get(It.IsAny<int>())).Returns(new Service());
            ServicesController servicesController = new(mockServices.Object);

            //Act
            var result = servicesController.Edit(1);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
            Assert.IsType<Service>(result.Model);
            Assert.Equal("AddOrEdit", result.ViewName);
            Assert.Equal("Изменить", result.ViewData["Title"]);
        }

        [Fact]
        public void Edit_PostResult()
        {
            //Arrange
            var mockServices = new Mock<IRepositoryOfServices>();
            ServicesController servicesController = new(mockServices.Object);

            //Act
            var result = servicesController.Edit(new Service());

            //Asset
            Assert.NotNull(result);
            Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", result.ActionName);
        }

        [Fact]
        public void DeleteResult()
        {
            //Arrange
            var mockServices = new Mock<IRepositoryOfServices>();
            mockServices.Setup(x=>x.Delete(It.IsAny<int>())).Returns(1);
            ServicesController servicesController = new(mockServices.Object);

            //Act
            var result = servicesController.Delete(1);

            //Asset
            Assert.NotNull(result);
            Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", result.ActionName);
        }

        [Fact]
        public void Pictures()
        {
            //Arrange
            var mockServices = new Mock<IRepositoryOfServices>();
            ServicesController servicesController = new(mockServices.Object);

            //Act
            var result = servicesController.Pictures();

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
            Assert.IsType<List<string>>(result.Model);
        }

        [Fact]
        public void AddPicturesAsyncResult()
        {
            //Arrange
            var mockServices = new Mock<IRepositoryOfServices>();
            ServicesController servicesController = new(mockServices.Object);

            //Act
            var result = servicesController.AddPicturesAsync(new List<IFormFile>());

            //Assert
            Assert.NotNull(result);
            Assert.IsType<Task<RedirectToActionResult>>(result);
            Assert.Equal("Pictures", result.Result.ActionName);
        }


        [Fact]
        public void DeletePicturesResult()
        {
            //Arrange
            var mockServices = new Mock<IRepositoryOfServices>();
            ServicesController servicesController = new(mockServices.Object);

            //Act
            var result = servicesController.DeletePicture("a");

            //Assert
            Assert.NotNull(result);
            Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Pictures", result.ActionName);
        }


        private List<Service> RepositoryOfService_GetAll() 
        {  
            return new() {
                new Service { Id = 1, Name = "Полубокс", Price = 100, TimeOfService = new TimeSpan(1,20,0), Description = "Под полубоксера" , Picture = "pictures/1.jpg"},
                new Service { Id = 2, Name = "Тенис", Price = 200, TimeOfService = new TimeSpan(0,25,0), Description = "Под тенисиста" , Picture = "pictures/2.jpg"},
                new Service { Id = 3, Name = "Модельная", Price = 300, TimeOfService = new TimeSpan(0,30,0), Description = "Под модель" , Picture = "pictures/3.jpg"},
                new Service { Id = 4, Name = "Котовский", Price = 400, TimeOfService = new TimeSpan(0,15,0), Description = "Под Котовского" , Picture = "pictures/4.jpg"},
                new Service { Id = 5, Name = "Гаршок", Price = 500, TimeOfService = new TimeSpan(0,10,0), Description = "Под горшок" , Picture = "pictures/5.jpg"},
            };
        }
    }
}
