﻿using System.Text;

namespace HairSalon.Model.Services
{
    public  class PicturesManager
    {
        private string _picturesDirectory;

        public PicturesManager()
        {
            _picturesDirectory = Directory.GetCurrentDirectory() + "/wwwroot/pictures";
        }

        public List<string> GetPictures()
        {
            var pictures = Directory.GetFiles(_picturesDirectory);

            var result = pictures.Select(x=> @"/pictures/" + Path.GetFileName(x)).ToList();

            return new(result.ToList());
        }

        public async Task UploadPicturesAsync(IEnumerable<IFormFile> files)
        {

            foreach (IFormFile item in files)
            {
                StringBuilder pref = new();

                //если файл с таким названием уже есть, то добавляем в начало названия букву i
                while (GetPictures().Select(x=> Path.GetFileName(x).ToLower()).Contains(pref.ToString() + (item.FileName).ToLower()))
                {
                    pref.Append("i");
                }

                string fileFullPath = _picturesDirectory + "/" + pref.ToString() + item.FileName;



                using (var fileStream = new FileStream(fileFullPath, FileMode.Create))
                {
                    await item.CopyToAsync(fileStream);
                }
            }

            return;
        } 

        public int DeletePicture(string fileShortPath)
        {
            try
            {
                string fileName = Path.GetFileName(fileShortPath);
                string filePaht = _picturesDirectory + "/" + fileName;

                File.Delete(filePaht);
                return 1;
            }
            catch (Exception)
            {
                return -1;
            }
        }
    }
}
