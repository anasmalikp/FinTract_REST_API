using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;

namespace FinTract_REST_API.Encryption
{
    public class PasswordHasher
    {
        public static string HashPassword(string password)
        {
            var salt = BCrypt.Net.BCrypt.GenerateSalt();
            return BCrypt.Net.BCrypt.HashPassword(password, salt);
        }

        public static bool VerifyPassword(string hashed, string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashed);
        }

        public static int DecodeToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var decoded = tokenHandler.ReadJwtToken(token);
            var userid = decoded.Claims.FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
            return int.Parse(userid);
        }
    }
}
