using HairSalon.Controllers.Admin;
using HairSalon.Model.Employees;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace HairSalon.Tests
{
    public class EmployeesControllerTest
    {
        [Fact]
        public void IndexResult()
        {
            //Arrange
            var mock = new Mock<IRepositoryOfEmployees>();
            mock.Setup(x => x.GetAll()).Returns(RepositoryOfEmployees_GetAll());
            EmployeesController  employeesController = new(mock.Object);

            //Act
            var result = employeesController.Index();

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
            Assert.Equal(2, (result.Model as List<Employee>)?.Count);
        }

        [Fact]
        public void Add_PostResult()
        {
            //Arrange
            var mock1 = new Mock<IRepositoryOfEmployees>();
            mock1.Setup(x => x.Add(It.IsAny<Employee>())).Returns(1);
            EmployeesController employeesController1 = new(mock1.Object);
            var mock2 = new Mock<IRepositoryOfEmployees>();
            mock2.Setup(x => x.Add(It.IsAny<Employee>())).Returns(-1);
            EmployeesController employeesController2 = new(mock2.Object);

            //Act
            var result1 = employeesController1.Add(new());
            var result2 = employeesController2.Add(new());

            //Assert
            Assert.NotNull(result1);
            Assert.NotNull(result2);
            Assert.IsType<RedirectToActionResult>(result1);
            Assert.IsType<RedirectToActionResult> (result2);
            Assert.Equal("Index", result1.ActionName);
            Assert.Equal("Index", result2.ActionName);

        }

        [Fact]
        public void Add_GetResult() 
        {
            //Arrange
            var mock = new Mock<IRepositoryOfEmployees>();
            EmployeesController employeesController = new(mock.Object);

            //Act
            var result = employeesController.Add();

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
            Assert.Equal("AddOrEdit", result.ViewName);
            Assert.Equal("Добавить", result.ViewData["Title"]);
        }

        [Fact]
        public void Edit_PostResult()
        {
            //Arrange
            var mock = new Mock<IRepositoryOfEmployees>();
            mock.Setup(x=>x.Update(It.IsAny<Employee>())).Returns(1);
            EmployeesController employeesController = new(mock.Object);

            //Act
            var result = employeesController.Edit(new Employee());


            //Assert
            Assert.NotNull(result);
            Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", result.ActionName);
        }


        [Fact]
        public void Edit_GetResult()
        {
            //Arrange
            var mock = new Mock<IRepositoryOfEmployees>();
            mock.Setup(x=>x.Get(It.IsAny<int>())).Returns(new Employee());
            EmployeesController employeesController = new(mock.Object);

            //Act
            var result = employeesController.Edit(1);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
            Assert.Equal("Изменить", result.ViewData["Title"]);
            Assert.IsType<Employee>(result.Model);
        }


        [Fact]
        public void DeleteResult()
        {
            //Arrange
            var mock = new Mock<IRepositoryOfEmployees>();
            mock.Setup(x => x.Delete(It.IsAny<int>())).Returns(1);
            EmployeesController employeesController = new(mock.Object);

            //Act
            var result = employeesController.Delete(1);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", result.ActionName);
        }

        private List<Employee> RepositoryOfEmployees_GetAll()
        {
            return new List<Employee>
            {
                new(){ Id = 1, Name = "Виктория", Post = "Парикмахер"},
                new(){ Id = 2, Name = "Елизавета", Post = "Парикмахер"},
            };
        }
    }
}
