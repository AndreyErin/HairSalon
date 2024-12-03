using HairSalon.Model.Records;
using HairSalon.Model.Records.Admin;
using Moq;
using Xunit;

namespace HairSalon.Tests
{
    public class RecordViewModelAdapterTest
    {
        [Fact]
        public void SetTimeTableResult()
        {
            //Arrange
            var mockRecords1 = new Mock<IRepositoryOfRecords>();
            mockRecords1.Setup(x => x.GetAll()).Returns(RecordsRepository_GetAll);
            mockRecords1.Setup(x => x.Add(It.IsAny<Model.Records.Record>())).Returns(1);
            mockRecords1.Setup(x => x.Update(It.IsAny<Model.Records.Record>())).Returns(1);
            mockRecords1.Setup(x => x.Delete(It.IsAny<int>())).Returns(1);
            RecordViewModelAdapter rvmAdapter1 = new(mockRecords1.Object);

            var mockRecords2 = new Mock<IRepositoryOfRecords>();
            mockRecords2.Setup(x => x.GetAll()).Returns(RecordsRepository_GetAll);
            mockRecords2.Setup(x => x.Add(It.IsAny<Model.Records.Record>())).Returns(-1);//неудача
            mockRecords2.Setup(x => x.Update(It.IsAny<Model.Records.Record>())).Returns(-1);
            mockRecords2.Setup(x => x.Delete(It.IsAny<int>())).Returns(-1);
            RecordViewModelAdapter rvmAdapter2 = new(mockRecords2.Object);

            //Act
            var result1 = rvmAdapter1.SetTimeTable(CreateTimeForRecordModel());
            var result2 = rvmAdapter2.SetTimeTable(CreateTimeForRecordModel());

            //Assert
            Assert.IsType<int>(result1);
            Assert.Equal(1, result1);

            Assert.IsType<int>(result2);
            Assert.Equal(-1, result2);
        }


        [Fact]
        public void SetWorkDatesResult()
        {
            //Arrange
            var mockRecords1 = new Mock<IRepositoryOfRecords>();
            mockRecords1.Setup(x => x.AddDayForRecords(It.IsAny<DateOnly>())).Returns(1);
            mockRecords1.Setup(x => x.DeleteDayForRecords(It.IsAny<DateOnly>())).Returns(1);
            RecordViewModelAdapter rvmAdapter1 = new(mockRecords1.Object);

            var mockRecords2 = new Mock<IRepositoryOfRecords>();
            mockRecords2.Setup(x => x.AddDayForRecords(It.IsAny<DateOnly>())).Returns(-1);
            mockRecords2.Setup(x => x.DeleteDayForRecords(It.IsAny<DateOnly>())).Returns(-1);
            RecordViewModelAdapter rvmAdapter2 = new(mockRecords2.Object);

            //Act
            var result1 = rvmAdapter1.SetWorkDates(CreateWorkDatesModel());
            var result2 = rvmAdapter2.SetWorkDates(CreateWorkDatesModel());

            //Assert
            Assert.IsType<int>(result1);
            Assert.Equal(1, result1);

            Assert.IsType<int>(result2);
            Assert.Equal(-1, result2);
        }


        private TimeForRecordModel[] CreateTimeForRecordModel()
        {
            DateOnly date = new(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            TimeOnly startTime = new(10,0);
            TimeOnly endTime = new(18,0);
            TimeOnly currentTime = startTime;
            List<TimeForRecordModel> resultList = new();

            while (currentTime < endTime)
            {
                resultList.Add(new() { Date = date, EmployeeId = 1, isEnable = false, Time = currentTime, Record = new() });

                currentTime = currentTime.AddMinutes(30);
            }

            return resultList.ToArray();
        }

        private WorkDatesModel[] CreateWorkDatesModel()
        {
            DateTime dt = DateTime.Now;
            DateOnly date = new(dt.Year, dt.Month, dt.Day);
            List<WorkDatesModel> result = new();

            for (int i = 0; i < 31; i++)
            {
                result.Add(new() { Day = date.AddDays(i), IsEnable = (i % 2  == 0)});
            }


            return result.ToArray();
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
    }
}
