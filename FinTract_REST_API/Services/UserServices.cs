using Dapper;
using FinTract_REST_API.Encryption;
using FinTract_REST_API.Interfaces;
using FinTract_REST_API.Models;
using IdentityModel.Client;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Drawing;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace FinTract_REST_API.Services
{
    public class UserServices:IUserServices
    {
        private readonly IDbConnection connection;
        private readonly IConfiguration config;
        private readonly ILogger<UserServices> logger;
        public UserServices(IConfiguration config, ILogger<UserServices> logger)
        {
            this.config = config;
            connection = new SqlConnection(config.GetConnectionString("DefaultConnection"));
            this.logger = logger;
        }

        public async Task<int?> RegisterUser(Users user)
        {
            try
            {
                user.password = PasswordHasher.HashPassword(user.password);
                var parameters = new DynamicParameters();
                parameters.Add("@username", user.username);
                parameters.Add("@email", user.email);
                parameters.Add("@password", user.password);
                parameters.Add("@Newuserid", dbType: DbType.Int32, direction:ParameterDirection.Output);
                parameters.Add("@Errormsg", dbType: DbType.String, size:200, direction:ParameterDirection.Output);

                await connection.ExecuteAsync("user_insert", parameters, commandType: CommandType.StoredProcedure);

                int? newUserid = parameters.Get<int?>("@Newuserid");
                string? Errormsg = parameters.Get<string?>("@Errormsg");

                if(newUserid == null)
                {
                    logger.LogError(Errormsg);
                    return -1;
                }
                return newUserid;
            }
            catch(Exception ex)
            {
                logger.LogError(ex.Message);
                return -1;
            }
        }

        public async Task<string> LoginUser(Users user)
        {
            try
            {
                var parameter = new DynamicParameters();
                parameter.Add("@email", user.email);

                var userfromdb = await connection.QueryAsync<Users>("Get_Users", parameter, commandType: CommandType.StoredProcedure);
                if(userfromdb == null)
                {
                    logger.LogError("User not registered");
                    return null;
                }
                var isVerified = PasswordHasher.VerifyPassword(userfromdb.FirstOrDefault().password, user.password);
                if (isVerified)
                {
                    return GetToken(userfromdb.FirstOrDefault());
                }
                logger.LogError("wrong password");
                return null;
            }
            catch(Exception ex)
            {
                logger.LogError(ex.Message);
                return null;
            }
        }

        private string GetToken(Users user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["jwt:key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.id.ToString())
            };

            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: credentials,
                expires: DateTime.Now.AddDays(1)
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
