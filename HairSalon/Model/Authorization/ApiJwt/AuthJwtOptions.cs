using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace HairSalon.Model.Authorization.Api_Jwt
{
    public class AuthJwtOptions
    {
        public static string ISSUER = "AuthServer"; // издатель токена
        public static string AUDIENCE = "AuthClient"; // потребитель токена
        private static string KEY = "mysupersecret_secretkey!123000000000000";   // ключ 32бита
        public static int LIFETIME = 5; // время жизни токена (в минутах)
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
