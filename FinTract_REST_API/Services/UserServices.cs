using Dapper;
using FinTract_REST_API.Encryption;
using FinTract_REST_API.Interfaces;
using FinTract_REST_API.Models;
using IdentityModel.Client;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Drawing;
using System.Reflection.Metadata;
using System.Security.Cryptography;

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

        public async Task<bool> RegisterUser(Users user)
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

                logger.LogInformation(Errormsg);
                return true;
            }
            catch(Exception ex)
            {
                logger.LogError(ex.Message);
                return false;
            }
        }
    }
}
