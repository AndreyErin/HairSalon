using HairSalon.Model.Services;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace HairSalon.Tests
{
    public class PicturesManagerTest
    {
        private PicturesManager pm;
        public PicturesManagerTest()
        {
            pm = new PicturesManager();
        }

        [Fact]
        public void GetAllResult()
        {
            //рассматривается только положительный сценарий
            //Act
            var result = pm.GetAll();

            //Assert
            Assert.NotNull(result);
            Assert.IsType<List<string>>(result);
        }

        [Fact]
        public async Task UploadAsyncResult()
        {
            //рассматривается только негативный сценарий
            //Act
            var result = await pm.UploadAsync(new List<IFormFile>()); ;

            //Assert
            Assert.IsType<int>(result);
            Assert.Equal(-1, result);
        }

        [Fact]
        public void DeleteResult()
        {
            //рассматривается только негативный сценарий
            //Act
            var result = pm.Delete("111");

            //Assert
            Assert.IsType<int>(result);
            Assert.Equal(-1, result);
        }
    }
}
