using Microsoft.AspNetCore.Identity;

namespace HairSalon.Model.Authorization
{
    public class User: IdentityUser
    {
        public int Id {  get; set; }
        public string Name { get; set; }
        public string Phone {  get; set; }
    }
}
