using HairSalon.Model.Records;
using HairSalon.Model;
using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using HairSalon.Model.Configuration;
using HairSalon.Controllers.Api;

namespace HairSalon.Tests
{
    public class RecordsApiControllerTest
    {
        [Fact]
        public void GetAllResultData()
        {
            //Arrange
            var mockRecords = new Mock<IRepositoryOfRecords>();
            mockRecords.Setup(repo => repo.GetAll()).Returns(GetAllRecords);
            var mockConfig = new Mock<IRepositoryOfConfiguration>();
            RecordsApiController recordsApiController = new(mockRecords.Object, mockConfig.Object);

            //Act
            PackageMessage? packageMessage = recordsApiController.GetAll().Value as PackageMessage;         
            List<Model.Records.Record>? records = packageMessage?.Data as List<Model.Records.Record>;
            var resultCount = records?.Count;

            //Assert
            Assert.Equal(3, resultCount);
            Assert.True(packageMessage?.Succeed);
            Assert.Equal("Елена", records?.FirstOrDefault(s => s.Id == 2)?.ClientName);
        }

        [Fact]
        public void GetResultDataInt()
        {
            //Arrange
            int id = 1, id2 = 20;
            var mockRecords = new Mock<IRepositoryOfRecords>();
            mockRecords.Setup(repo => repo.Get(id)).Returns(GetAllRecords().FirstOrDefault(r=>r.Id == id));
            //недостижимый id2
            mockRecords.Setup(repo => repo.Get(id2)).Returns(GetAllRecords().FirstOrDefault(r => r.Id == id2));

            var mockConfig = new Mock<IRepositoryOfConfiguration>();
            RecordsApiController recordsApiController = new(mockRecords.Object, mockConfig.Object);

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
            var mockRecords = new Mock<IRepositoryOfRecords>();
            mockRecords.Setup(repo => repo.Get(name1)).Returns(GetAllRecords().FirstOrDefault(r => r.ClientName == name1));
            //недостижимое имя name2
            mockRecords.Setup(repo => repo.Get(name2)).Returns(GetAllRecords().FirstOrDefault(r => r.ClientName == name2));

            var mockConfig = new Mock<IRepositoryOfConfiguration>();
            RecordsApiController recordsApiController = new(mockRecords.Object, mockConfig.Object);

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
            var mockRecords = new Mock<IRepositoryOfRecords>();
            mockRecords.Setup(repo => repo.Delete(id)).Returns(1);
            //недостижимый id2
            mockRecords.Setup(repo => repo.Delete(id2)).Returns(0);

            var mockConfig = new Mock<IRepositoryOfConfiguration>();
            RecordsApiController recordsApiController = new(mockRecords.Object, mockConfig.Object);

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
            Model.Records.Record record1 = new() { Id = 11, ClientName = "Лейла", SeviceName = "Каре", DateForVisit = new(2025, 02, 15), TimeForVisit = new(10, 0, 0) };
            Model.Records.Record record2 = new() { Id = 12, ClientName = "Мария", SeviceName = "Модельная", DateForVisit = new(2025, 01, 15), TimeForVisit = new(10, 0, 0) };

            var mockRecords = new Mock<IRepositoryOfRecords>();
            mockRecords.Setup(repo => repo.Add(record1)).Returns(1);
            //отрицательный результат
            mockRecords.Setup(repo => repo.Add(record2)).Returns(0);

            var mockConfig = new Mock<IRepositoryOfConfiguration>();
            RecordsApiController recordsApiController = new(mockRecords.Object, mockConfig.Object);

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
            var mockRecords = new Mock<IRepositoryOfRecords>();
            mockRecords.Setup(d=>d.GetDaysForRecords()).Returns(days);

            var mockConfig = new Mock<IRepositoryOfConfiguration>();
            RecordsApiController recordsApiController = new(mockRecords.Object, mockConfig.Object);

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

        [Fact]
        public void GetFreeTimeForRecordsResult()
        {
            //Arrange
            var mockRecords = new Mock<IRepositoryOfRecords>();
            mockRecords.Setup(r => r.GetAll()).Returns(GetAllRecords);
            mockRecords.Setup(r => r.GetDaysForRecords()).Returns(GetDaysForRecords());
            var mockConfig = new Mock<IRepositoryOfConfiguration>();
            mockConfig.Setup(c=>c.GetConfig()).Returns(GetConfig());
            RecordsApiController recordsApiController = new(mockRecords.Object, mockConfig.Object);

            //Act
            JsonResult jsonResult1 = recordsApiController.GetFreeTimeForRecords(65, 1);
            PackageMessage? packageMessage1 = jsonResult1.Value as PackageMessage;
            List<FreeTimeForRecords>? freeTimeForRecords1 = packageMessage1?.Data as List<FreeTimeForRecords>;

            JsonResult jsonResult2 = recordsApiController.GetFreeTimeForRecords(30, 1);
            PackageMessage? packageMessage2 = jsonResult2.Value as PackageMessage;
            List<FreeTimeForRecords>? freeTimeForRecords2 = packageMessage2?.Data as List<FreeTimeForRecords>;

            //Assert
            Assert.True(packageMessage1?.Succeed);
            Assert.Equal(5, freeTimeForRecords1?.Count);
            //для услуги длительностью 65 минут доступны 11 вариантов времени для записи,
            //при условии, что 11:00 уже заняты услугой на 20 минут
            Assert.Equal(11, freeTimeForRecords1?[0].Times.Count);
            //для услуги длительностью 65 минут доступны 14 вариантов времени для записи,
            //при условии, что весь день свободен для записи
            Assert.Equal(14, freeTimeForRecords1?[4].Times.Count);

            Assert.True(packageMessage2?.Succeed);
            Assert.Equal(5, freeTimeForRecords2?.Count);
            //для услуги длительностью 30 минут доступны 15 вариантов времени для записи
            //при условии, что 11:00 уже заняты услугой на 20 минут
            Assert.Equal(15, freeTimeForRecords2?[0].Times.Count);
            //для услуги длительностью 30 минут доступны 16 вариантов времени для записи,
            //при условии, что весь день свободен для записи
            Assert.Equal(16, freeTimeForRecords2?[4].Times.Count);

        }

        private Config GetConfig()
        {
            return new() 
            {
                MobileAppEnabled = true,
                RecordEnable = true,
                PromotionEnabled = true,
                NumberOfDaysForRecords = 10, 
                StartTimeOfDaty = new(10,00), 
                EndTimeOfDaty = new(18, 00)               
            };
        }

        private List<DateOnly> GetDaysForRecords()
        {
            DateOnly toDay = new(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            List<DateOnly> result = new();
            //5 дней подрят начиная с завтрашнего
            //сегодняшний день проверять нельзя, тк в зависимоти от времени дня
            //запись то будет, то нет
            for (int i = 0; i < 5; i++)
            {
                toDay = toDay.AddDays(1);
                result.Add(toDay);
            }

            return result;
        }

        private List<Model.Records.Record> GetAllRecords()
        {
            DateOnly toDay = new( DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            return new() 
            { 
                new Model.Records.Record 
                { 
                    Id = 1,
                    ClientName = "Маша",
                    ClientPhone = "9600000000",
                    DateForVisit = toDay.AddDays(1),
                    TimeForVisit = new(11,00),
                    SeviceName = "Модельная",
                    DurationOfService = 20,
                    EmployeeId = 1
                } ,
                new Model.Records.Record
                {
                    Id = 2,
                    ClientName = "Елена",
                    ClientPhone = "9600000000",
                    DateForVisit = toDay.AddDays(2),
                    TimeForVisit = new(12,00),
                    SeviceName = "Каре",
                    DurationOfService = 70,
                    EmployeeId = 1
                } ,
                new Model.Records.Record
                {
                    Id = 3,
                    ClientName = "Николай",
                    ClientPhone = "9600000000",
                    DateForVisit = toDay.AddDays(3),
                    TimeForVisit = new(15,00),
                    SeviceName = "Полубокс",
                    DurationOfService = 50,
                    EmployeeId = 1
                } 
            };
        }


    }
}
