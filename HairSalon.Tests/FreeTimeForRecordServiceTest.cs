using HairSalon.Model.Configuration;
using HairSalon.Model.Records;
using HairSalon.Model.Records.Api;
using Moq;
using Xunit;

namespace HairSalon.Tests
{
    public class FreeTimeForRecordServiceTest
    {
        [Fact]
        public void GetFreeTimes()
        {
            //Arrange
            var mockRecords = new Mock<IRepositoryOfRecords>();
            mockRecords.Setup(r=>r.GetDaysForRecords()).Returns(GetDaysForRecords());
            mockRecords.Setup(r => r.GetAll()).Returns(GetAllRecords());
            var mockConfig = new Mock<IRepositoryOfConfiguration>();
            mockConfig.Setup(r=>r.GetConfig()).Returns(GetConfig());
            FreeTimeForRecordService timeForRecordService = new(mockRecords.Object, mockConfig.Object);

            //Act
            var result1 = timeForRecordService.GetFreeTimes(25, 1);//25 минут
            var result2 = timeForRecordService.GetFreeTimes(90, 1);//90 минут

            //Assert
            Assert.NotNull(result1);
            Assert.IsType<List<FreeTimeForRecords>>(result1);
            Assert.Equal(16, result1[4].Times.Count);
            Assert.Equal(15, result1[0].Times.Count);
            Assert.Equal(5, result1.Count);

            Assert.NotNull(result2);
            Assert.IsType<List<FreeTimeForRecords>>(result2);
            Assert.Equal(14, result2[4].Times.Count);
            Assert.Equal(11, result2[0].Times.Count);
            Assert.Equal(5, result2.Count);
        }

        private Config GetConfig()
        {
            return new()
            {
                MobileAppEnabled = true,
                RecordEnable = true,
                PromotionEnabled = true,
                NumberOfDaysForRecords = 10,
                StartTimeOfDay = new(10, 00),
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
            DateOnly toDay = new(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

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
