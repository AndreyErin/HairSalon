using System.Text;

namespace HairSalon.Model.Services
{
    public interface IPicturesManager
    {
        public List<string> GetPictures();

        public Task<int> UploadPicturesAsync(IEnumerable<IFormFile> files);

        public int DeletePicture(string fileShortPath);

    }
}