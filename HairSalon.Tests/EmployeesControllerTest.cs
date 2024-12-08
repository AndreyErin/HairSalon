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
            mock2.Setup(x => x.Add(It.IsAny<Employee>())).Returns(0);
            EmployeesController employeesController2 = new(mock2.Object);

            //Act
            var result1 = employeesController1.Add(new());
            var result2 = employeesController2.Add(new());

            //Assert
            Assert.NotNull(result1);
            Assert.IsType<RedirectToActionResult>(result1);
            Assert.Equal("Index", result1.ActionName);
            Assert.Equal("Employees", result1.ControllerName);

            Assert.NotNull(result2);
            Assert.IsType<RedirectToActionResult> (result2);
            Assert.Equal("ErrorPage", result2.ActionName);
            Assert.Equal("Admin", result2.ControllerName);
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
            var mock1 = new Mock<IRepositoryOfEmployees>();
            mock1.Setup(x=>x.Update(It.IsAny<Employee>())).Returns(1);
            EmployeesController employeesController1 = new(mock1.Object);

            var mock2 = new Mock<IRepositoryOfEmployees>();
            mock2.Setup(x => x.Update(It.IsAny<Employee>())).Returns(0);
            EmployeesController employeesController2 = new(mock2.Object);

            //Act
            var result1 = employeesController1.Edit(new Employee());
            var result2 = employeesController2.Edit(new Employee());

            //Assert
            Assert.NotNull(result1);
            Assert.IsType<RedirectToActionResult>(result1);
            Assert.Equal("Index", result1.ActionName);
            Assert.Equal("Employees", result1.ControllerName);

            Assert.NotNull(result2);
            Assert.IsType<RedirectToActionResult>(result2);
            Assert.Equal("ErrorPage", result2.ActionName);
            Assert.Equal("Admin", result2.ControllerName);
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
            var mock1 = new Mock<IRepositoryOfEmployees>();
            mock1.Setup(x => x.Delete(It.IsAny<int>())).Returns(1);
            EmployeesController employeesController1 = new(mock1.Object);

            var mock2 = new Mock<IRepositoryOfEmployees>();
            mock2.Setup(x => x.Delete(It.IsAny<int>())).Returns(0);
            EmployeesController employeesController2 = new(mock2.Object);

            //Act
            var result1 = employeesController1.Delete(1);
            var result2 = employeesController1.Delete(1);

            //Assert
            Assert.NotNull(result1);
            Assert.IsType<RedirectToActionResult>(result1);
            Assert.Equal("Index", result1.ActionName);
            Assert.Equal("Employees", result1.ControllerName);

            Assert.NotNull(result2);
            Assert.IsType<RedirectToActionResult>(result2);
            Assert.Equal("Index", result2.ActionName);
            Assert.Equal("Employees", result2.ControllerName);
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
