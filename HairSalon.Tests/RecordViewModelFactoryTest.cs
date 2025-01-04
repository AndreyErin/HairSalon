using HairSalon.Model.Configuration;
using HairSalon.Model.Employees;
using HairSalon.Model.Records;
using HairSalon.Model.Records.Admin;
using Moq;
using Xunit;

namespace HairSalon.Tests
{
    public class RecordViewModelFactoryTest
    {
        [Fact]
        public void CreateWorkDatesModelArrayResutl()
        {
            //Arrange
            var dt = DateTime.Now;
            var mockConfig = new Mock<IRepositoryOfConfiguration>();
            var mockEmployees = new Mock<IRepositoryOfEmployees>();
            var mockRecords = new Mock<IRepositoryOfRecords>();
            mockRecords.Setup(x=>x.GetDaysForRecords()).Returns(RecordsRepository_GetDaysForRecords);
            RecordViewModelFactory rvmFactory = new(mockRecords.Object, mockEmployees.Object, mockConfig.Object);

            //Act
            var result = rvmFactory.CreateWorkDatesModelArray();

            //Assert
            Assert.NotNull(result);
            Assert.IsType<WorkDatesModel[]>(result);
            Assert.Equal(31, result?.Length);
            Assert.Equal(new DateOnly(dt.Year, dt.Month, dt.Day).AddDays(3), result?[3].Day);
        }

        [Fact]
        public void CreateRecordsForEmployeeAllDaysModelListResult()
        {
            //Arrange
            var mockConfig = new Mock<IRepositoryOfConfiguration>();
            var mockRecords = new Mock<IRepositoryOfRecords>();
            mockRecords.Setup(x=>x.GetAll()).Returns(RecordsRepository_GetAll);
            var mockEmployees = new Mock<IRepositoryOfEmployees>();
            mockEmployees.Setup(x=>x.GetAll()).Returns(EmployeesRepository_GetAll);
            RecordViewModelFactory rvmFactory = new(mockRecords.Object, mockEmployees.Object, mockConfig.Object);

            //Act
            var result = rvmFactory.CreateRecordsForEmployeeAllDaysModelList();

            //Assert
            Assert.NotNull(result);
            Assert.IsType<List<RecordsForEmployeeAllModel>>(result);
            Assert.Equal(2, result.Count); //2 набора т.к. 2 сотрудника

            Assert.Equal("Виктория", result[0].EmployeeName);
            Assert.Equal(2, result[0].RecordsOfDays.Count); //записи разбиты на 2 дня
            Assert.Equal(2, result[0].RecordsOfDays[0].Records.Count); //2 записи в первый день
            Assert.Equal(1, result[0].RecordsOfDays[1].Records.Count); //одна запись во второй день

            Assert.Equal("Елизавета", result[1].EmployeeName);
            Assert.Equal(1, result[1].RecordsOfDays.Count); //одна запись в один день(запись на вчерашщний день игнорится)
        }

        [Fact]
        public void CreateRecordsForEmployeeOfDayModelListResult()
        {
            //Arrange
            DateTime tomorrow = DateTime.Now.AddDays(1);
            DateOnly date = new(tomorrow.Year, tomorrow.Month, tomorrow.Day);
            var mockConfig = new Mock<IRepositoryOfConfiguration>();
            mockConfig.Setup(x => x.GetConfig()).Returns(EmployeesRepository_GetConfig);
            var mockRecords = new Mock<IRepositoryOfRecords>();
            mockRecords.Setup(x => x.GetAll()).Returns(RecordsRepository_GetAll);
            var mockEmployees = new Mock<IRepositoryOfEmployees>();
            mockEmployees.Setup(x => x.GetAll()).Returns(EmployeesRepository_GetAll);
            RecordViewModelFactory rvmFactory = new(mockRecords.Object, mockEmployees.Object, mockConfig.Object);

            //Act
            var result = rvmFactory.CreateRecordsForEmployeeOfDayModelList(date);


            //Assert
            Assert.NotNull(result);
            Assert.IsType<List<RecordsForEmployeeOfDayModel>>(result);
            Assert.Equal(2, result.Count); //2 сотрудника = 2 элемента в листе

            Assert.Equal(16, result[0].RecordsOfDay.Records.Count); //16 отрезков по 30 минут 

            Assert.Equal("Виктория", result[0].Employee.Name);
            Assert.Equal("Парикмахер", result[0].Employee.Post);
            Assert.Equal(1, result[0].Employee.Id);

            Assert.Equal("Елизавета", result[1].Employee.Name);
            Assert.Equal("Парикмахер", result[1].Employee.Post);
            Assert.Equal(2, result[1].Employee.Id);
        }

        public void CreateTimeForRecordModelArrayResult()
        {
            //Arrange
            DateTime tomorrow = DateTime.Now.AddDays(1);
            DateOnly date = new(tomorrow.Year, tomorrow.Month, tomorrow.Day);
            var mockConfig = new Mock<IRepositoryOfConfiguration>();
            mockConfig.Setup(x => x.GetConfig()).Returns(EmployeesRepository_GetConfig);
            var mockRecords = new Mock<IRepositoryOfRecords>();
            mockRecords.Setup(x => x.GetAll()).Returns(RecordsRepository_GetAll);
            var mockEmployees = new Mock<IRepositoryOfEmployees>();
            mockEmployees.Setup(x => x.GetAll()).Returns(EmployeesRepository_GetAll);
            RecordViewModelFactory rvmFactory = new(mockRecords.Object, mockEmployees.Object, mockConfig.Object);

            //Act
            var result = rvmFactory.CreateTimeForRecordModelArray(date, 1);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<TimeForRecordModel[]>(result);

            Assert.Equal(16, result.Length);
            Assert.True(result[0].isEnable);
            Assert.False(result[1].isEnable);
            Assert.Equal("Мария", result[0].Record?.ClientName);//есть запись
            Assert.Null(result[1].Record);//время свободно
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
                new(){Id = 1, ClientName = "Мария", ClientPhone = "9600000000", SeviceName = "Модельная", DurationOfService = 20, DateForVisit = new(dt2.Year, dt2.Month, dt2.Day), TimeForVisit = new ( 10, 0, 0), EmployeeId = 1},
                new(){Id = 2, ClientName = "Томара", ClientPhone = "9600000000", SeviceName = "Модельная", DurationOfService = 30, DateForVisit = new(dt2.Year, dt2.Month, dt2.Day), TimeForVisit = new ( 15, 0, 0), EmployeeId = 1},
                new(){Id = 3, ClientName = "Елена", ClientPhone = "9600000000", SeviceName = "Каре",  DurationOfService = 20, DateForVisit = new(dt2.Year, dt2.Month, dt2.Day), TimeForVisit = new ( 10, 30, 0), EmployeeId = 2},
                new(){Id = 4, ClientName = "Николай", ClientPhone = "9600000000", SeviceName = "Полубокс",  DurationOfService = 20, DateForVisit = new(dt4.Year, dt4.Month, dt4.Day), TimeForVisit = new(11, 0, 0), EmployeeId = 1},
                new(){Id = 5, ClientName = "Филип", ClientPhone = "9600000000", SeviceName = "Полубокс",  DurationOfService = 20, DateForVisit = new(dt5.Year, dt5.Month, dt5.Day), TimeForVisit = new(10, 0, 0), EmployeeId = 2}

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

        private Config EmployeesRepository_GetConfig()
        {
            return new() {  
                MobileAppEnabled = false,
                PromotionEnabled = false,
                RecordEnable = false,
                EndTimeOfDay = new(18, 0),
                StartTimeOfDay = new(10, 0),
                NumberOfDaysForRecords = 7};
        }
    }
}
