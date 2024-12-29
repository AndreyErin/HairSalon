using HairSalon.Controllers.Api;
using HairSalon.Model;
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
            JsonResult jsonResult = employeesApiController.GetAll();
            PackageMessage? packageMessage = jsonResult.Value as PackageMessage;
            List<Employee>? resutlEmployees = packageMessage?.Data as List<Employee>;

            //Assert
            Assert.NotNull(packageMessage);
            Assert.True(packageMessage?.Succeed);
            Assert.Null(packageMessage?.ErrorText);           
            Assert.NotNull(resutlEmployees);
            Assert.Equal(2, resutlEmployees?.Count);
            Assert.Equal("Елизавета", resutlEmployees?.FirstOrDefault(e=>e.Id == 2)?.Name);
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
            JsonResult jsonResult1 = employeesApiController.Get(id);
            JsonResult jsonResult2 = employeesApiController.Get(id2);
            PackageMessage? packageMessage1 = jsonResult1.Value as PackageMessage;
            PackageMessage? packageMessage2 = jsonResult2.Value as PackageMessage;

            //Assert
            Assert.NotNull(packageMessage1);
            Assert.NotNull(packageMessage2);

            Assert.True(packageMessage1?.Succeed);
            Assert.False(packageMessage2?.Succeed);

            Assert.Null(packageMessage1?.ErrorText);
            Assert.NotNull(packageMessage2?.ErrorText);

            Assert.NotNull(packageMessage1?.Data as Employee);
            Assert.Null(packageMessage2?.Data as Employee);

            Assert.Equal("Виктория", (packageMessage1?.Data as Employee)?.Name);
        }

        [Fact]
        public void GetResultString()
        {
            //Arrange
            string name1 = "Виктория", name2 = "Аркадий";
            var mock = new Mock<IRepositoryOfEmployees>();
            mock.Setup(e => e.Get(name1)).Returns(GetAllEmployees().FirstOrDefault(r => r.Name == name1));
            //недостижимое имя name2
            mock.Setup(e => e.Get(name2)).Returns(GetAllEmployees().FirstOrDefault(r => r.Name == name2));
            EmployeesApiController employeesApiController = new(mock.Object);

            //Act
            JsonResult jsonResult1 = employeesApiController.Get(name1);
            JsonResult jsonResult2 = employeesApiController.Get(name2);
            PackageMessage? packageMessage1 = jsonResult1.Value as PackageMessage;
            PackageMessage? packageMessage2 = jsonResult2.Value as PackageMessage;

            //Assert
            Assert.NotNull(packageMessage1);
            Assert.NotNull(packageMessage2);

            Assert.True(packageMessage1?.Succeed);
            Assert.False(packageMessage2?.Succeed);

            Assert.Null(packageMessage1?.ErrorText);
            Assert.NotNull(packageMessage2?.ErrorText);

            Assert.NotNull(packageMessage1?.Data as Employee);
            Assert.Null(packageMessage2?.Data as Employee);

            Assert.Equal("Виктория", (packageMessage1?.Data as Employee)?.Name);
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
            JsonResult jsonResult1 = employeesApiController.Update(employee1);
            JsonResult jsonResult2 = employeesApiController.Update(employee2);
            PackageMessage? packageMessage1 = jsonResult1.Value as PackageMessage;
            PackageMessage? packageMessage2 = jsonResult2.Value as PackageMessage;

            //Assert
            Assert.NotNull(packageMessage1);
            Assert.NotNull(packageMessage2);

            Assert.True(packageMessage1?.Succeed);
            Assert.False(packageMessage2?.Succeed);

            Assert.Null(packageMessage1?.ErrorText);
            Assert.NotNull(packageMessage2?.ErrorText);

            Assert.Null(packageMessage1?.Data as Employee);
            Assert.Null(packageMessage2?.Data as Employee);
        }

        [Fact]
        public void AddResult()
        {
            //Arrange            
            Employee employee1 = new() { Id = 55, Name = "Тимофей", Post = "Охранник" };
            Employee employee2 = new() { Id = 1, Name = "Виктория", Post = "Парикмахер" };
            var mock = new Mock<IRepositoryOfEmployees>();
            mock.Setup(e => e.Add(employee1)).Returns(1);
            //несуществующий работник
            mock.Setup(e => e.Add(employee2)).Returns(0);
            EmployeesApiController employeesApiController = new(mock.Object);

            //Act
            ObjectResult jsonResult1 = employeesApiController.Add(employee1);
            ObjectResult jsonResult2 = employeesApiController.Add(employee2);


            var a = jsonResult1?.StatusCode;
            var d = jsonResult1?.Value;

            PackageMessage? packageMessage1 = jsonResult1?.Value as PackageMessage;
            PackageMessage? packageMessage2 = jsonResult2?.Value as PackageMessage;

            //Assert
            Assert.NotNull(packageMessage1);
            Assert.NotNull(packageMessage2);

            Assert.True(packageMessage1?.Succeed);
            Assert.False(packageMessage2?.Succeed);

            Assert.Null(packageMessage1?.ErrorText);
            Assert.NotNull(packageMessage2?.ErrorText);

            Assert.Null(packageMessage1?.Data as Employee);
            Assert.Null(packageMessage2?.Data as Employee);
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
            JsonResult jsonResult1 = employeesApiController.Delete(id);
            JsonResult jsonResult2 = employeesApiController.Delete(id2);
            PackageMessage? packageMessage1 = jsonResult1.Value as PackageMessage;
            PackageMessage? packageMessage2 = jsonResult2.Value as PackageMessage;

            //Assert
            Assert.NotNull(packageMessage1);
            Assert.NotNull(packageMessage2);

            Assert.True(packageMessage1?.Succeed);
            Assert.False(packageMessage2?.Succeed);

            Assert.Null(packageMessage1?.ErrorText);
            Assert.NotNull(packageMessage2?.ErrorText);

            Assert.Null(packageMessage1?.Data as Employee);
            Assert.Null(packageMessage2?.Data as Employee);
        }
    }
}
