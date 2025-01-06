using HairSalon.Model.Records;
using HairSalon.Model;
using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using HairSalon.Model.Configuration;
using HairSalon.Model.Records.Api;
using HairSalon.Controllers.Api.v1;
using Microsoft.AspNetCore.Http.HttpResults;

namespace HairSalon.Tests
{
    public class RecordsApiControllerTest
    {
        [Fact]
        public void GetAllResultData()
        {
            //Arrange
            var mockRecords1 = new Mock<IRepositoryOfRecords>();
            mockRecords1.Setup(repo => repo.GetAll()).Returns(GetAllRecords);
            var mockRecords2 = new Mock<IRepositoryOfRecords>();
            mockRecords2.Setup(repo => repo.GetAll()).Returns(new List<Model.Records.Record>());
            var mockConfig = new Mock<IRepositoryOfConfiguration>();
            RecordsApiController recordsApiController1 = new(mockRecords1.Object, mockConfig.Object);
            RecordsApiController recordsApiController2 = new(mockRecords2.Object, mockConfig.Object);

            //Act
            var resultOk = recordsApiController1.GetAll();
            var resultNotFound = recordsApiController2.GetAll();

            //Assert
            Assert.NotNull(resultOk);
            Assert.IsType<OkObjectResult>(resultOk);
            Assert.IsType<List<Model.Records.Record>>((resultOk as OkObjectResult)?.Value);

            Assert.NotNull(resultNotFound);
            Assert.IsType<NotFoundObjectResult>(resultNotFound);
            Assert.IsType<string>((resultNotFound as ObjectResult)?.Value);
        }

        [Fact]
        public void GetResult()
        {
            //Arrange
            int id1 = 1, id2 = 20 , id3 = -5;
            var mockRecords = new Mock<IRepositoryOfRecords>();
            mockRecords.Setup(repo => repo.Get(id1)).Returns(GetAllRecords().FirstOrDefault(r=>r.Id == id1));
            //недостижимый id2
            mockRecords.Setup(repo => repo.Get(id2)).Returns(GetAllRecords().FirstOrDefault(r => r.Id == id2));

            var mockConfig = new Mock<IRepositoryOfConfiguration>();
            RecordsApiController recordsApiController = new(mockRecords.Object, mockConfig.Object);

            //Act
            var resultOk = recordsApiController.Get(id1);
            var resultNotFound = recordsApiController.Get(id2);
            var resultUnprocessableEntity = recordsApiController.Get(id3);

            //Assert
            Assert.NotNull(resultOk);
            Assert.IsType<OkObjectResult>(resultOk);
            Assert.IsType<Model.Records.Record>((resultOk as OkObjectResult)?.Value);

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
            int id1 = 1, id2 = 20, id3 = -8;
            var mockRecords = new Mock<IRepositoryOfRecords>();
            mockRecords.Setup(repo => repo.Delete(id1)).Returns(1);
            //недостижимый id2
            mockRecords.Setup(repo => repo.Delete(id2)).Returns(0);

            var mockConfig = new Mock<IRepositoryOfConfiguration>();
            RecordsApiController recordsApiController = new(mockRecords.Object, mockConfig.Object);

            //Act
            var resultOk = recordsApiController.Delete(id1);
            var resultNotFound = recordsApiController.Delete(id2);
            var resultUnprocessableEntity = recordsApiController.Delete(id3);

            //Assert
            Assert.NotNull(resultOk);
            Assert.IsType<OkResult>(resultOk);
            Assert.Null(resultOk as ObjectResult);

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
            var dt1 = DateTime.Now;
            var dt2 = DateTime.Now.AddDays(1);
            var dt3 = DateTime.Now.AddDays(3);
            var dt4 = DateTime.Now.AddDays(4);

            Model.Records.Record record1 = new() { Id = 0, ClientName = "Лейла", ServiceName = "Каре", DateForVisit = new(dt1.Year, dt1.Month, dt1.Day), TimeForVisit = new(10, 0, 0), ClientPhone = "fdfd", DurationOfService = 10, EmployeeId = 1 };
            Model.Records.Record record2 = new() { Id = 0, ClientName = "Мария", ServiceName = "Модельная", DateForVisit = new(dt2.Year, dt2.Month, dt2.Day), TimeForVisit = new(10, 0, 0), ClientPhone = "fdfd", DurationOfService = 10, EmployeeId = 1 };
            Model.Records.Record record3 = new(); 

            var mockRecords = new Mock<IRepositoryOfRecords>();
            mockRecords.Setup(repo => repo.Add(record1)).Returns(1);
            //отрицательный результат
            mockRecords.Setup(repo => repo.Add(record2)).Returns(0);

            var mockConfig = new Mock<IRepositoryOfConfiguration>();
            RecordsApiController recordsApiController = new(mockRecords.Object, mockConfig.Object);

            //Act
            var resultOk = recordsApiController.Add(record1);
            var resultConflict = recordsApiController.Add(record2);
            var resultUnprocessableEntity = recordsApiController.Add(record3);

            //Assert
            Assert.NotNull(resultOk);
            Assert.IsType<OkObjectResult>(resultOk);
            Assert.IsType<Model.Records.Record>((resultOk as ObjectResult)?.Value);

            Assert.NotNull(resultConflict);
            Assert.IsType<ConflictObjectResult>(resultConflict);
            Assert.IsType<string>((resultConflict as ObjectResult)?.Value);

            Assert.NotNull(resultUnprocessableEntity);
            Assert.IsType<UnprocessableEntityObjectResult>(resultUnprocessableEntity);
            Assert.IsType<string>((resultUnprocessableEntity as ObjectResult)?.Value);
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
            var mockRecords1 = new Mock<IRepositoryOfRecords>();
            mockRecords1.Setup(d=>d.GetDaysForRecords()).Returns(days);
            var mockRecords2 = new Mock<IRepositoryOfRecords>();
            mockRecords2.Setup(d => d.GetDaysForRecords()).Returns(new List<DateOnly>());

            var mockConfig = new Mock<IRepositoryOfConfiguration>();
            RecordsApiController recordsApiController1 = new(mockRecords1.Object, mockConfig.Object);
            RecordsApiController recordsApiController2 = new(mockRecords2.Object, mockConfig.Object);

            //Act
            var resultOk = recordsApiController1.GetDaysForRecords();
            var resultNotFound = recordsApiController2.GetDaysForRecords();

            //Assert
            Assert.NotNull(resultOk);
            Assert.IsType<OkObjectResult>(resultOk);
            Assert.IsType<List<DateOnly>>((resultOk as ObjectResult)?.Value);

            Assert.NotNull(resultNotFound);
            Assert.IsType<NotFoundObjectResult>(resultNotFound);
            Assert.IsType<string>((resultNotFound as ObjectResult)?.Value);
        }

        [Fact]
        public void GetFreeTimeForRecordsResult()
        {
            //Arrange
            var mockRecords1 = new Mock<IRepositoryOfRecords>();
            mockRecords1.Setup(r => r.GetAll()).Returns(GetAllRecords);
            mockRecords1.Setup(r => r.GetDaysForRecords()).Returns(GetDaysForRecords());

            var mockRecords2 = new Mock<IRepositoryOfRecords>();
            mockRecords2.Setup(r => r.GetAll()).Returns(GetAllRecords);
            mockRecords2.Setup(r => r.GetDaysForRecords()).Returns(new List<DateOnly>());

            var mockConfig = new Mock<IRepositoryOfConfiguration>();
            mockConfig.Setup(c=>c.GetConfig()).Returns(GetConfig());
            RecordsApiController recordsApiController1 = new(mockRecords1.Object, mockConfig.Object);
            RecordsApiController recordsApiController2 = new(mockRecords2.Object, mockConfig.Object);

            //Act
            var resultOk = recordsApiController1.GetFreeTimeForRecords(65, 1);
            var resultNotFound = recordsApiController2.GetFreeTimeForRecords(20, 1);
            var resultUnprocessableEntity = recordsApiController1.GetFreeTimeForRecords(0, 0);

            //Assert
            Assert.NotNull(resultOk);
            Assert.IsType<OkObjectResult>(resultOk);
            Assert.IsType<List<FreeTimeForRecords>>((resultOk as ObjectResult)?.Value);

            Assert.NotNull(resultNotFound);
            Assert.IsType<NotFoundObjectResult>(resultNotFound);
            Assert.IsType<string>((resultNotFound as ObjectResult)?.Value);

            Assert.NotNull(resultUnprocessableEntity);
            Assert.IsType<UnprocessableEntityObjectResult>(resultUnprocessableEntity);
            Assert.IsType<string>((resultUnprocessableEntity as ObjectResult)?.Value);
        }

        private Config GetConfig()
        {
            return new() 
            {
                MobileAppEnabled = true,
                RecordEnable = true,
                PromotionEnabled = true,
                NumberOfDaysForRecords = 10, 
                StartTimeOfDay = new(10,00), 
                EndTimeOfDay = new(18, 00)               
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
                    ServiceName = "Модельная",
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
                    ServiceName = "Каре",
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
                    ServiceName = "Полубокс",
                    DurationOfService = 50,
                    EmployeeId = 1
                } 
            };
        }


    }
}
