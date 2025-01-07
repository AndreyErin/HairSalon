using System.Text;

namespace HairSalon.Model.Services
{
    public interface IPicturesManager
    {
        public List<string> GetAll();

        public Task<int> UploadAsync(IEnumerable<IFormFile> files);

        public int Delete(string fileShortPath);

    }
}