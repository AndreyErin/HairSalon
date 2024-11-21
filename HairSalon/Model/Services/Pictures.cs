namespace HairSalon.Model.Services
{
    public static class Pictures
    {
        public static List<string> GetPictures()
        {
            var pictures = Directory.GetFiles(Directory.GetCurrentDirectory() + "/wwwroot/pictures");

            var result = pictures.Select(x=> Path.GetFileName(x)).ToList();

            return new(result.ToList());
        }
    }
}
