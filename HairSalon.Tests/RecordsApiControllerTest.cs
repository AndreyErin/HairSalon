using HairSalon.Controllers;
using HairSalon.Model.Records;
using HairSalon.Model;
using Moq;
using Xunit;

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
            Assert.Equal("Елена", records?.FirstOrDefault(s => s.Id == 2)?.Name);
        }

        private List<Model.Records.Record> GetAllRecords() 
        {
            return  new List<Model.Records.Record>
            {
                new(){Id = 1, Name = "Мария", SeviceName = "Модельная", DateTimeOfRecord = new DateTime(2025, 01, 15, 10, 0, 0)},
                new(){Id = 2, Name = "Елена", SeviceName = "Каре", DateTimeOfRecord = new DateTime(2025, 01, 15, 10, 30, 0)},
                new(){Id = 3, Name = "Николай", SeviceName = "Полубокс", DateTimeOfRecord = new DateTime(2025, 01, 15, 11, 0, 0)}
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
            mock.Setup(repo => repo.Get(name1)).Returns(GetAllRecords().FirstOrDefault(r => r.Name == name1));
            //недостижимый id2
            mock.Setup(repo => repo.Get(name2)).Returns(GetAllRecords().FirstOrDefault(r => r.Name == name2));
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
    }
}
