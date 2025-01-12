using HairSalon.Controllers.Admin;
using HairSalon.Model.Configuration;
using HairSalon.Model.Employees;
using HairSalon.Model.Records;
using HairSalon.Model.Records.Admin;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace HairSalon.Tests
{
    public class RecordsControllerTest
    {
        [Fact]
        public void IndexResult()
        {
            //Arrange
            var mockConfig = new Mock<IRepositoryOfConfiguration>();
            var mockEmployees = new Mock<IRepositoryOfEmployees>();
            var mockRecords = new Mock<IRepositoryOfRecords>();
            mockRecords.Setup(x => x.GetDaysForRecords()).Returns(RecordsRepository_GetDaysForRecords);
            RecordsController recordsController = new(mockRecords.Object, mockEmployees.Object, mockConfig.Object);

            //Act
            var result = recordsController.Index();

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
            Assert.IsType<WorkDatesModel[]>(result.Model);
        }

        [Fact]
        public void SetDaysForRecordsResutl()
        {
            //Arrange 
            WorkDatesModel[] model =
            {
                    new() { Day = new(), IsEnable = true },
                    new() { Day = new(), IsEnable = false }
            };
            var mockConfig = new Mock<IRepositoryOfConfiguration>();
            var mockEmployees = new Mock<IRepositoryOfEmployees>();
            var mockRecords = new Mock<IRepositoryOfRecords>();
            RecordsController recordsController = new(mockRecords.Object, mockEmployees.Object, mockConfig.Object);


            //Act
            var result1 = recordsController.SetDaysForRecords(model);
            var result2 = recordsController.SetDaysForRecords(Array.Empty<WorkDatesModel>());

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
        public void DayResult()
        {
            //Arrange
            var mockConfig = new Mock<IRepositoryOfConfiguration>();
            mockConfig.Setup(x=>x.GetConfig()).Returns(ConfigurationRepository_GetConfig);
            var mockEmployees = new Mock<IRepositoryOfEmployees>();
            mockEmployees.Setup(x=>x.GetAll()).Returns(EmployeesRepository_GetAll);
            var mockRecords = new Mock<IRepositoryOfRecords>();
            mockRecords.Setup(x => x.GetAll()).Returns(RecordsRepository_GetAll);
            RecordsController recordsController = new(mockRecords.Object, mockEmployees.Object, mockConfig.Object);

            //Act
            var result = recordsController.Day(new DateOnly().ToString());

            //Assetr
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
            Assert.IsType<List<RecordsForEmployeeOfDayModel>>(result.Model);
        }

        [Fact]
        public void EditTimeOfDayForEmployeeResult()
        {
            //Arrange
            var mockConfig = new Mock<IRepositoryOfConfiguration>();
            mockConfig.Setup(x => x.GetConfig()).Returns(ConfigurationRepository_GetConfig);
            var mockEmployees = new Mock<IRepositoryOfEmployees>();
            mockEmployees.Setup(x => x.Get(It.IsAny<int>())).Returns(EmployeesRepository_GetAll()[0]);
            var mockRecords = new Mock<IRepositoryOfRecords>();
            mockRecords.Setup(x => x.GetAll()).Returns(RecordsRepository_GetAll);
            RecordsController recordsController = new(mockRecords.Object, mockEmployees.Object, mockConfig.Object);

            //Act
            var result = recordsController.EditTimeOfDayForEmployee(new DateOnly().ToString(), 1);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
            Assert.IsType<TimeForRecordModel[]>(result.Model);
            Assert.Equal("Виктория", result.ViewData["EmployeeName"]);
        }

        [Fact]
        public void SetTimeOfDayForEmployeeResult() 
        {
            //Arrange
            TimeForRecordModel[] model =
            {
                new TimeForRecordModel(){ isEnable = false},
                new TimeForRecordModel(){ isEnable = true}
            };
            var mockConfig = new Mock<IRepositoryOfConfiguration>();
            var mockEmployees = new Mock<IRepositoryOfEmployees>();
            var mockRecords1 = new Mock<IRepositoryOfRecords>();
            mockRecords1.Setup(x => x.GetAll()).Returns(RecordsRepository_GetAll);
            mockRecords1.Setup(x => x.Add(It.IsAny<Model.Records.Record>())).Returns(1);
            mockRecords1.Setup(x => x.Delete(It.IsAny<int>())).Returns(1);
            mockRecords1.Setup(x => x.Update(It.IsAny<Model.Records.Record>())).Returns(1);
            RecordsController recordsController1 = new(mockRecords1.Object, mockEmployees.Object, mockConfig.Object);

            var mockRecords2 = new Mock<IRepositoryOfRecords>();
            mockRecords2.Setup(x => x.GetAll()).Returns(RecordsRepository_GetAll);
            mockRecords2.Setup(x => x.Add(It.IsAny<Model.Records.Record>())).Returns(-1);
            mockRecords2.Setup(x => x.Delete(It.IsAny<int>())).Returns(-1);
            mockRecords2.Setup(x => x.Update(It.IsAny<Model.Records.Record>())).Returns(-1);
            RecordsController recordsController2 = new(mockRecords2.Object, mockEmployees.Object, mockConfig.Object);

            //Act
            var result1 = recordsController1.SetTimeOfDayForEmployee(model);
            var result2 = recordsController2.SetTimeOfDayForEmployee(model);

            //Assert
            Assert.NotNull(result1);
            Assert.IsType<RedirectToActionResult>(result1);
            Assert.Equal("Index", result1.ActionName);

            Assert.NotNull(result2);
            Assert.IsType<RedirectToActionResult>(result2);
            Assert.Equal("ErrorPage", result2.ActionName);
            Assert.Equal("Admin", result2.ControllerName);
        }



        private List<DateOnly> RecordsRepository_GetDaysForRecords()
        {
            var dt1 = DateTime.Now;
            var dt2 = DateTime.Now.AddDays(1);
            var dt3 = DateTime.Now.AddDays(3);
            var dt4 = DateTime.Now.AddDays(4);

            return new()
            {
                new(dt1.Year, dt1.Month, dt1.Day),
                new(dt2.Year, dt2.Month, dt2.Day),
                new(dt3.Year, dt3.Month, dt3.Day),
                new(dt4.Year, dt4.Month, dt4.Day)
            };
        }

        private List<Model.Records.Record> RecordsRepository_GetAll()
        {
            var dt1 = DateTime.Now;
            var dt2 = DateTime.Now.AddDays(1);
            var dt3 = DateTime.Now.AddDays(3);
            var dt4 = DateTime.Now.AddDays(4);
            var dt5 = DateTime.Now.AddDays(-1);//вчерашний день. не полжен попасть в выдачу

            return new()
            {
                new(){Id = 1, ClientName = "Мария", ClientPhone = "9600000000", ServiceName = "Модельная", DurationOfService = 20, DateForVisit = new(dt2.Year, dt2.Month, dt2.Day), TimeForVisit = new ( 10, 0, 0), EmployeeId = 1},
                new(){Id = 2, ClientName = "Томара", ClientPhone = "9600000000", ServiceName = "Модельная", DurationOfService = 30, DateForVisit = new(dt2.Year, dt2.Month, dt2.Day), TimeForVisit = new ( 15, 0, 0), EmployeeId = 1},
                new(){Id = 3, ClientName = "Елена", ClientPhone = "9600000000", ServiceName = "Каре",  DurationOfService = 20, DateForVisit = new(dt2.Year, dt2.Month, dt2.Day), TimeForVisit = new ( 10, 30, 0), EmployeeId = 2},
                new(){Id = 4, ClientName = "Николай", ClientPhone = "9600000000", ServiceName = "Полубокс",  DurationOfService = 20, DateForVisit = new(dt4.Year, dt4.Month, dt4.Day), TimeForVisit = new(11, 0, 0), EmployeeId = 1},
                new(){Id = 5, ClientName = "Филип", ClientPhone = "9600000000", ServiceName = "Полубокс",  DurationOfService = 20, DateForVisit = new(dt5.Year, dt5.Month, dt5.Day), TimeForVisit = new(10, 0, 0), EmployeeId = 2}

            };
        }

        private List<Employee> EmployeesRepository_GetAll()
        {
            return new List<Employee>
            {
                new(){ Id = 1, Name = "Виктория", Post = "Парикмахер"},
                new(){ Id = 2, Name = "Елизавета", Post = "Парикмахер"},
            };
        }

        private Config ConfigurationRepository_GetConfig()
        {
            return new()
            {
                MobileAppEnabled = true,
                PromotionEnabled = true,
                RecordEnabled = true,
                NumberOfDaysForRecords = 7,
                StartTimeOfDay = new(),
                EndTimeOfDay = new()
            };
        }
    }
}
