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
            var mockPictures = new Mock<IPicturesManager>();
            var mockServices = new Mock<IRepositoryOfServices>();
            mockServices.Setup(x=>x.GetAll()).Returns(RepositoryOfService_GetAll);
            ServicesController servicesController = new(mockServices.Object, mockPictures.Object);

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
            var mockPictures = new Mock<IPicturesManager>();
            var mockServices = new Mock<IRepositoryOfServices>();
            ServicesController servicesController = new(mockServices.Object, mockPictures.Object);

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
            var mockPictures = new Mock<IPicturesManager>();
            var mockServices1 = new Mock<IRepositoryOfServices>();
            mockServices1.Setup(x => x.Add(It.IsAny<Service>())).Returns(1);
            ServicesController servicesController1 = new(mockServices1.Object, mockPictures.Object);

            var mockServices2 = new Mock<IRepositoryOfServices>();
            mockServices2.Setup(x => x.Add(It.IsAny<Service>())).Returns(-1);
            ServicesController servicesController2 = new(mockServices2.Object, mockPictures.Object);

            //Act
            var result1 = servicesController1.Add(new());
            var result2 = servicesController2.Add(new());

            //Assert
            Assert.NotNull(result1);
            Assert.IsType<RedirectToActionResult>(result1);
            Assert.Equal("Index", result1.ActionName);

            Assert.NotNull(result2);
            Assert.IsType<RedirectToActionResult>(result2);
            Assert.Equal("ErrorPage", result2.ActionName);
            Assert.Equal("Admin", result2.ControllerName);
        }


        [Fact]
        public void Edit_GetResult()
        {
            //Arrange
            var mockPictures = new Mock<IPicturesManager>();
            var mockServices = new Mock<IRepositoryOfServices>();
            mockServices.Setup(x => x.Get(It.IsAny<int>())).Returns(new Service());
            ServicesController servicesController = new(mockServices.Object, mockPictures.Object);

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
            var mockPictures = new Mock<IPicturesManager>();
            var mockServices1 = new Mock<IRepositoryOfServices>();
            mockServices1.Setup(x => x.Update(It.IsAny<Service>())).Returns(1);
            ServicesController servicesController1 = new(mockServices1.Object, mockPictures.Object);

            var mockServices2 = new Mock<IRepositoryOfServices>();
            mockServices2.Setup(x => x.Update(It.IsAny<Service>())).Returns(-1);
            ServicesController servicesController2 = new(mockServices2.Object, mockPictures.Object);

            //Act
            var result1 = servicesController1.Edit(new Service());
            var result2 = servicesController2.Edit(new Service());

            //Asset
            Assert.NotNull(result1);
            Assert.IsType<RedirectToActionResult>(result1);
            Assert.Equal("Index", result1.ActionName);

            Assert.NotNull(result2);
            Assert.IsType<RedirectToActionResult>(result2);
            Assert.Equal("ErrorPage", result2.ActionName);
            Assert.Equal("Admin", result2.ControllerName);
        }

        [Fact]
        public void DeleteResult()
        {
            //Arrange
            var mockPictures = new Mock<IPicturesManager>();
            var mockServices1 = new Mock<IRepositoryOfServices>();
            mockServices1.Setup(x => x.Delete(It.IsAny<int>())).Returns(1);
            ServicesController servicesController1 = new(mockServices1.Object, mockPictures.Object);

            var mockServices2 = new Mock<IRepositoryOfServices>();
            mockServices2.Setup(x => x.Delete(It.IsAny<int>())).Returns(-1);
            ServicesController servicesController2 = new(mockServices2.Object, mockPictures.Object);

            //Act
            var result1 = servicesController1.Delete(1);
            var result2 = servicesController2.Delete(1);

            //Asset
            Assert.NotNull(result1);
            Assert.IsType<RedirectToActionResult>(result1);
            Assert.Equal("Index", result1.ActionName);

            Assert.NotNull(result2);
            Assert.IsType<RedirectToActionResult>(result2);
            Assert.Equal("ErrorPage", result2.ActionName);
            Assert.Equal("Admin", result2.ControllerName);
        }

        [Fact]
        public void Pictures()
        {
            //Arrange
            var mockPictures = new Mock<IPicturesManager>();
            mockPictures.Setup(x => x.GetAll()).Returns(new List<string>());
            var mockServices = new Mock<IRepositoryOfServices>();
            ServicesController servicesController = new(mockServices.Object, mockPictures.Object);

            //Act
            var result = servicesController.Pictures();

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
            Assert.IsType<List<string>>(result.Model);
        }

        [Fact]
        public void UploadAsyncResult()
        {
            //Arrange
            var mockPictures1 = new Mock<IPicturesManager>();
            mockPictures1.Setup(x => x.UploadAsync(new List<IFormFile>())).Returns(new Task<int>(() => 1));
            var mockPictures2 = new Mock<IPicturesManager>();
            mockPictures2.Setup(x => x.UploadAsync(new List<IFormFile>())).Returns(new Task<int>(() => -1));
            var mockServices = new Mock<IRepositoryOfServices>();
            ServicesController servicesController1 = new(mockServices.Object, mockPictures1.Object);
            ServicesController servicesController2 = new(mockServices.Object, mockPictures2.Object);


            //Act
            var result1 = servicesController1.AddPicturesAsync(new List<IFormFile>());
            var result2 = servicesController2.AddPicturesAsync(new List<IFormFile>());

            //Assert
            Assert.NotNull(result1);
            //Assert.IsType<Task<RedirectToActionResult>>(result2);
            //Assert.Equal("Pictures", result2?.Result.ActionName);
            //Assert.Equal("Admin", result2?.Result.ControllerName);

            Assert.NotNull(result2);
            //Assert.IsType<Task<RedirectToActionResult>>(result2);
            //Assert.Equal("ErrorPage", result2?.Result.ActionName);
            //Assert.Equal("Admin", result2?.Result.ControllerName);
        }


        [Fact]
        public void DeletePicturesResult()
        {
            //Arrange
            var mockPictures = new Mock<IPicturesManager>();
            mockPictures.Setup(x => x.Delete("true")).Returns(1);
            mockPictures.Setup(x => x.Delete("false")).Returns(-1);
            var mockServices = new Mock<IRepositoryOfServices>();
            ServicesController servicesController = new(mockServices.Object, mockPictures.Object);

            //Act
            var result1 = servicesController.DeletePicture("true");
            var result2 = servicesController.DeletePicture("false");

            //Assert
            Assert.NotNull(result1);
            Assert.IsType<RedirectToActionResult>(result1);
            Assert.Equal("Pictures", result1.ActionName);

            Assert.NotNull(result2);
            Assert.IsType<RedirectToActionResult>(result2);
            Assert.Equal("ErrorPage", result2.ActionName);
            Assert.Equal("Admin", result2.ControllerName);
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
