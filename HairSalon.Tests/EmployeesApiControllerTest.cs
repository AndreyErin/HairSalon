using HairSalon.Controllers.Api.v1;
using HairSalon.Model.Employees;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace HairSalon.Tests
{
    public class EmployeesApiControllerTest
    {

        [Fact]
        public void GetAllReturn()
        {
            //Arrange
            var mock = new Mock<IRepositoryOfEmployees>();
            mock.Setup(e => e.GetAll()).Returns(GetAllEmployees);
            EmployeesApiController employeesApiController = new(mock.Object);

            //Act
            var result = employeesApiController.GetAll();

            //Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
            Assert.IsType<List<Employee>>((result as ObjectResult)?.Value);
            Assert.Equal(2, ((result as ObjectResult)?.Value as List<Employee>)?.Count);
        }

        private List<Employee> GetAllEmployees()
        {
            return new()
            {
                new(){ Id = 1, Name = "Виктория", Post = "Парикмахер"},
                new(){ Id = 2, Name = "Елизавета", Post = "Парикмахер"},
            };
        }

        [Fact]
        public void GetResultInt()
        {
            //Arrange
            int id = 1, id2 = 20;
            var mock = new Mock<IRepositoryOfEmployees>();
            mock.Setup(e => e.Get(id)).Returns(GetAllEmployees().FirstOrDefault(r=>r.Id == id));
            //недостижимый id2
            mock.Setup(e => e.Get(id2)).Returns(GetAllEmployees().FirstOrDefault(r => r.Id == id2));
            EmployeesApiController employeesApiController = new(mock.Object);

            //Act
            var result1 = employeesApiController.Get(id);
            var result2 = employeesApiController.Get(id2);


            //Assert
            Assert.NotNull(result1);
            Assert.IsType<OkObjectResult>(result1);
            Assert.IsType<Employee>((result1 as ObjectResult)?.Value);

            Assert.NotNull(result2);
            Assert.IsType<NotFoundObjectResult>(result2);
            Assert.IsType<string>((result2 as ObjectResult)?.Value);
        }

        [Fact]
        public void UpdateResult()
        {
            //Arrange
            Employee employee1 = new() {Id = 1 , Name = "Вика", Post = "Стилист" };
            Employee employee2 = new() { Id = 55, Name = "Тимофей", Post = "Охранник" };
            var mock = new Mock<IRepositoryOfEmployees>();
            mock.Setup(e => e.Update(employee1)).Returns(1);
            //несуществующий работник
            mock.Setup(e => e.Update(employee2)).Returns(0);
            EmployeesApiController employeesApiController = new(mock.Object);

            //Act
            var result1 = employeesApiController.Update(employee1);
            var result2 = employeesApiController.Update(employee2);


            //Assert
            Assert.NotNull(result1);
            Assert.IsType<OkObjectResult>(result1);

            Assert.NotNull(result2);
            Assert.IsType<NotFoundObjectResult>(result2);
            Assert.IsType<string>((result2 as ObjectResult)?.Value);
        }

        [Fact]
        public void AddResult()
        {
            //Arrange            
            Employee employee1 = new() { Id = 55, Name = "Тимофей", Post = "Охранник" };
            Employee employee2 = new() { Id = 1, Name = "Виктория", Post = "Парикмахер" };
            var mock = new Mock<IRepositoryOfEmployees>();
            mock.Setup(e => e.Add(employee1)).Returns(1);
            //такой работник уже есть в базе
            mock.Setup(e => e.Add(employee2)).Returns(0);
            EmployeesApiController employeesApiController = new(mock.Object);

            //Act
            var result1 = employeesApiController.Add(employee1);
            var result2 = employeesApiController.Add(employee2);

            //Assert
            Assert.NotNull(result1);
            Assert.IsType<CreatedResult>(result1);

            Assert.NotNull(result2);
            Assert.IsType<ConflictObjectResult>(result2);
            Assert.IsType<string>((result2 as ConflictObjectResult)?.Value);
        }

        [Fact]
        public void DeleteResult()
        {
            //Arrange
            int id = 1, id2 = 20;
            var mock = new Mock<IRepositoryOfEmployees>();
            mock.Setup(e => e.Delete(id)).Returns(1);
            //недостижимый id2
            mock.Setup(e => e.Delete(id2)).Returns(0);
            EmployeesApiController employeesApiController = new(mock.Object);

            //Act
            var result1 = employeesApiController.Delete(id);
            var result2 = employeesApiController.Delete(id2);

            //Assert
            Assert.NotNull(result1);
            Assert.IsType<OkResult>(result1);

            Assert.NotNull(result2);
            Assert.IsType<NotFoundObjectResult>(result2);
            Assert.IsType<string>((result2 as NotFoundObjectResult)?.Value);
        }
    }
}
