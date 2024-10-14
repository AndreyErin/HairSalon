namespace HairSalon.Model
{
    public class PackageMessage
    {     
        public bool Succeed { get; set; }
        public object? Data { get; set; }
        public string? ErrorText { get; set; }
        public PackageMessage(bool succeed, object? data = null, string? errorText = null)
        {
            Data = data;
            Succeed = succeed;
            ErrorText = errorText;
        }
    }
}
