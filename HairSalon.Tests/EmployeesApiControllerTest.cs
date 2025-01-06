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
            var mockEmployees1 = new Mock<IRepositoryOfEmployees>();
            mockEmployees1.Setup(e => e.GetAll()).Returns(GetAllEmployees);
            EmployeesApiController employeesApiController1 = new(mockEmployees1.Object);
            
            var mockEmployees2 = new Mock<IRepositoryOfEmployees>();
            mockEmployees2.Setup(e => e.GetAll()).Returns(new List<Employee>());
            EmployeesApiController employeesApiController2 = new(mockEmployees2.Object);

            //Act
            var resultOk = employeesApiController1.GetAll();
            var resultNotFound = employeesApiController2.GetAll();

            //Assert
            Assert.NotNull(resultOk);
            Assert.IsType<OkObjectResult>(resultOk);
            Assert.IsType<List<Employee>>((resultOk as ObjectResult)?.Value);
            Assert.Equal(2, ((resultOk as ObjectResult)?.Value as List<Employee>)?.Count);

            Assert.NotNull(resultNotFound);
            Assert.IsType<NotFoundObjectResult>(resultNotFound);
            Assert.IsType<string>((resultNotFound as ObjectResult)?.Value);
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
        public void GetResult()
        {
            //Arrange
            int id1 = 1, id2 = 20, id3 = 0;
            var mock = new Mock<IRepositoryOfEmployees>();
            mock.Setup(e => e.Get(id1)).Returns(GetAllEmployees().FirstOrDefault(r=>r.Id == id1));
            //недостижимый id2
            mock.Setup(e => e.Get(id2)).Returns(GetAllEmployees().FirstOrDefault(r => r.Id == id2));
            EmployeesApiController employeesApiController = new(mock.Object);

            //Act
            var resultOk = employeesApiController.Get(id1);
            var resultNotFound = employeesApiController.Get(id2);
            var resultUnprocessableEntity = employeesApiController.Get(id3);

            //Assert
            Assert.NotNull(resultOk);
            Assert.IsType<OkObjectResult>(resultOk);
            Assert.IsType<Employee>((resultOk as ObjectResult)?.Value);

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
            Employee employee1 = new() { Id = 55, Name = "Тимофей", Post = "Охранник" };
            Employee employee2 = new() { Id = 1, Name = "Виктория", Post = "Парикмахер" };
            var mock = new Mock<IRepositoryOfEmployees>();
            mock.Setup(e => e.Add(employee1)).Returns(1);
            //такой работник уже есть в базе
            mock.Setup(e => e.Add(employee2)).Returns(0);
            EmployeesApiController employeesApiController = new(mock.Object);

            //Act
            var resultCreated = employeesApiController.Add(employee1);
            var resultConflict = employeesApiController.Add(employee2);
            var resultUnprocessableEntity = employeesApiController.Add(new());

            //Assert
            Assert.NotNull(resultCreated);
            Assert.IsType<CreatedResult>(resultCreated);
            Assert.IsType<Employee>((resultCreated as ObjectResult)?.Value);

            Assert.NotNull(resultConflict);
            Assert.IsType<ConflictObjectResult>(resultConflict);
            Assert.IsType<string>((resultConflict as ObjectResult)?.Value);

            Assert.NotNull(resultUnprocessableEntity);
            Assert.IsType<UnprocessableEntityObjectResult>(resultUnprocessableEntity);
            Assert.IsType<string>((resultUnprocessableEntity as ObjectResult)?.Value);
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
            var resultOk = employeesApiController.Update(employee1);
            var resultNotFound = employeesApiController.Update(employee2);
            var resultUnprocessableEntity = employeesApiController.Update(new());

            //Assert
            Assert.NotNull(resultOk);
            Assert.IsType<OkObjectResult>(resultOk);
            Assert.IsType<Employee>((resultOk as ObjectResult)?.Value);

            Assert.NotNull(resultNotFound);
            Assert.IsType<NotFoundObjectResult>(resultNotFound);
            Assert.IsType<string>((resultNotFound as ObjectResult)?.Value);

            Assert.NotNull(resultUnprocessableEntity);
            Assert.IsType<UnprocessableEntityObjectResult>(resultUnprocessableEntity);
            Assert.IsType<string>((resultUnprocessableEntity as ObjectResult)?.Value);
        }

        [Fact]
        public void DeleteResult()
        {
            //Arrange
            int id1 = 1, id2 = 20 , id3 = 0;
            var mock = new Mock<IRepositoryOfEmployees>();
            mock.Setup(e => e.Delete(id1)).Returns(1);
            //недостижимый id2
            mock.Setup(e => e.Delete(id2)).Returns(0);
            EmployeesApiController employeesApiController = new(mock.Object);

            //Act
            var resultOk = employeesApiController.Delete(id1);
            var resultNotFound = employeesApiController.Delete(id2);
            var resultUnprocessableEntity = employeesApiController.Delete(id3);

            //Assert
            Assert.NotNull(resultOk);
            Assert.IsType<OkResult>(resultOk);

            Assert.NotNull(resultNotFound);
            Assert.IsType<NotFoundObjectResult>(resultNotFound);
            Assert.IsType<string>((resultNotFound as NotFoundObjectResult)?.Value);

            Assert.NotNull(resultUnprocessableEntity);
            Assert.IsType<UnprocessableEntityObjectResult>(resultUnprocessableEntity);
            Assert.IsType<string>((resultUnprocessableEntity as ObjectResult)?.Value);
        }
    }
}
