using HairSalon.Controllers;
using HairSalon.Model.Records;
using HairSalon.Model;
using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;

namespace HairSalon.Tests
{
    public class RecordsApiControllerTest
    {
        [Fact]
        public void GetAllResultData()
        {
            //Arrange
            var mock = new Mock<IRepositoryOfRecords>();
            mock.Setup(repo => repo.GetAll()).Returns(GetAllRecords);
            RecordsApiController recordsApiController = new(mock.Object);

            //Act
            PackageMessage? packageMessage = recordsApiController.GetAll().Value as PackageMessage;         
            List<Model.Records.Record>? records = packageMessage?.Data as List<Model.Records.Record>;
            var resultCount = records?.Count;

            //Assert
            Assert.Equal(3, resultCount);
            Assert.True(packageMessage?.Succeed);
            Assert.Equal("Елена", records?.FirstOrDefault(s => s.Id == 2)?.ClientName);
        }

        private List<Model.Records.Record> GetAllRecords() 
        {
            return  new List<Model.Records.Record>
            {
                new(){Id = 1, ClientName = "Мария", ClientPhone = "9600000000", SeviceName = "Модельная", DurationOfService = 20, DateTimeForVisit = new DateTime(2025, 01, 15, 10, 0, 0), EmployeeId = 1},
                new(){Id = 2, ClientName = "Елена", ClientPhone = "9600000000", SeviceName = "Каре",  DurationOfService = 20, DateTimeForVisit = new DateTime(2025, 01, 15, 10, 30, 0), EmployeeId = 1},
                new(){Id = 3, ClientName = "Николай", ClientPhone = "9600000000", SeviceName = "Полубокс",  DurationOfService = 20, DateTimeForVisit = new DateTime(2025, 01, 15, 11, 0, 0), EmployeeId = 1}
            };
        }

        [Fact]
        public void GetResultDataInt()
        {
            //Arrange
            int id = 1, id2 = 20;
            var mock = new Mock<IRepositoryOfRecords>();
            mock.Setup(repo => repo.Get(id)).Returns(GetAllRecords().FirstOrDefault(r=>r.Id == id));
            //недостижимый id2
            mock.Setup(repo => repo.Get(id2)).Returns(GetAllRecords().FirstOrDefault(r => r.Id == id2));
            RecordsApiController recordsApiController = new(mock.Object);

            //Act
            PackageMessage? packageMessage1 = recordsApiController.Get(id).Value as PackageMessage;
            PackageMessage? packageMessage2 = recordsApiController.Get(id2).Value as PackageMessage;
            Model.Records.Record? result1 = packageMessage1?.Data as Model.Records.Record;
            Model.Records.Record? result2 = packageMessage2?.Data as Model.Records.Record;

            //Assert
            Assert.NotNull(result1);
            Assert.Null(result2);

            Assert.True(packageMessage1?.Succeed);
            Assert.False(packageMessage2?.Succeed);

            Assert.DoesNotContain("Ошибка", packageMessage1?.ErrorText);
            Assert.Contains("Ошибка", packageMessage2?.ErrorText);
            
        }

        [Fact]
        public void GetResultDataString()
        {
            //Arrange
            string name1 = "Николай", name2 = "Тимофей";
            var mock = new Mock<IRepositoryOfRecords>();
            mock.Setup(repo => repo.Get(name1)).Returns(GetAllRecords().FirstOrDefault(r => r.ClientName == name1));
            //недостижимое имя name2
            mock.Setup(repo => repo.Get(name2)).Returns(GetAllRecords().FirstOrDefault(r => r.ClientName == name2));
            RecordsApiController recordsApiController = new(mock.Object);

            //Act
            PackageMessage? packageMessage1 = recordsApiController.Get(name1).Value as PackageMessage;
            PackageMessage? packageMessage2 = recordsApiController.Get(name2).Value as PackageMessage;
            Model.Records.Record? result1 = packageMessage1?.Data as Model.Records.Record;
            Model.Records.Record? result2 = packageMessage2?.Data as Model.Records.Record;

            //Assert
            Assert.NotNull(result1);
            Assert.Null(result2);

            Assert.True(packageMessage1?.Succeed);
            Assert.False(packageMessage2?.Succeed);

            Assert.DoesNotContain("Ошибка", packageMessage1?.ErrorText);
            Assert.Contains("Ошибка", packageMessage2?.ErrorText);
        }

        [Fact]
        public void DeleteResult()
        {
            //Arrange
            int id = 1, id2 = 20;
            var mock = new Mock<IRepositoryOfRecords>();
            mock.Setup(repo => repo.Delete(id)).Returns(1);
            //недостижимый id2
            mock.Setup(repo => repo.Delete(id2)).Returns(0);
            RecordsApiController recordsApiController = new(mock.Object);

            //Act
            PackageMessage? packageMessage1 = recordsApiController.Delete(id).Value as PackageMessage;
            PackageMessage? packageMessage2 = recordsApiController.Delete(id2).Value as PackageMessage;
            Model.Records.Record? result1 = packageMessage1?.Data as Model.Records.Record;
            Model.Records.Record? result2 = packageMessage2?.Data as Model.Records.Record;

            //Assert
            Assert.True(packageMessage1?.Succeed);
            Assert.False(packageMessage2?.Succeed);

            Assert.True(packageMessage1?.Succeed);
            Assert.False(packageMessage2?.Succeed);

            Assert.DoesNotContain("Ошибка", packageMessage1?.ErrorText);
            Assert.Contains("Ошибка", packageMessage2?.ErrorText);
        }

        [Fact]
        public void AddResult()
        {
            //Arrange
            Model.Records.Record record1 = new() { Id = 11, ClientName = "Лейла", SeviceName = "Каре", DateTimeForVisit = new DateTime(2025, 02, 15, 10, 0, 0) };
            Model.Records.Record record2 = new() { Id = 12, ClientName = "Мария", SeviceName = "Модельная", DateTimeForVisit = new DateTime(2025, 01, 15, 10, 0, 0) };

            var mock = new Mock<IRepositoryOfRecords>();
            mock.Setup(repo => repo.Add(record1)).Returns(1);
            //отрицательный результат
            mock.Setup(repo => repo.Add(record2)).Returns(0);
            RecordsApiController recordsApiController = new(mock.Object);

            //Act
            PackageMessage? packageMessage1 = recordsApiController.Add(record1).Value as PackageMessage;
            PackageMessage? packageMessage2 = recordsApiController.Add(record2).Value as PackageMessage;
            Model.Records.Record? result1 = packageMessage1?.Data as Model.Records.Record;
            Model.Records.Record? result2 = packageMessage2?.Data as Model.Records.Record;

            //Assert
            Assert.True(packageMessage1?.Succeed);
            Assert.False(packageMessage2?.Succeed);

            Assert.True(packageMessage1?.Succeed);
            Assert.False(packageMessage2?.Succeed);

            Assert.DoesNotContain("Ошибка", packageMessage1?.ErrorText);
            Assert.Contains("Ошибка", packageMessage2?.ErrorText);
        }

        [Fact]
        public void GetDaysForRecordsResult()
        {
            //Arrange
            List<DateOnly> days = new() 
            {
                new(2025, 1 , 15),
                new(2025, 1 , 16),
                new(2025, 1 , 17)
            };
            var mock = new Mock<IRepositoryOfRecords>();
            mock.Setup(d=>d.GetDaysForRecords()).Returns(days);
            RecordsApiController recordsApiController = new(mock.Object);

            //Act
            JsonResult jsonResult = recordsApiController.GetDaysForRecords();
            PackageMessage? packageMessage = jsonResult.Value as PackageMessage;

            //Assert
            Assert.NotNull(packageMessage);
            Assert.NotNull(packageMessage.Data);
            Assert.True(packageMessage.Succeed);
            Assert.Equal(3, (packageMessage?.Data as List<DateOnly>)?.Count);
            Assert.Equal(new DateOnly(2025, 1, 17), (packageMessage?.Data as List<DateOnly>)?[2]);
        }
    }
}
