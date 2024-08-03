using Dapper;
using FinTract_REST_API.Interfaces;
using FinTract_REST_API.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace FinTract_REST_API.Services
{
    public class AccountServices:IAccountServices
    {
        private readonly IDbConnection connection;
        private readonly ILogger<AccountServices> logger;
        private readonly IConfiguration config;
        public AccountServices(IConfiguration config, ILogger<AccountServices> logger)
        {
            this.config = config;
            this.connection = new SqlConnection(config.GetConnectionString("DefaultConnection"));
        }
        public async Task<bool> CreateAccount(Accounts account)
        {
            try
            {
                var parameter = new DynamicParameters();
                parameter.Add("@TransactionName", account.TransactionName);
                parameter.Add("@UserId", account.UserId);
                parameter.Add("@catId", account.catId);
                parameter.Add("@NewTransId", dbType: DbType.Int32, direction: ParameterDirection.Output);
                parameter.Add("@Errormsg", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

                await connection.ExecuteAsync("Accounts_insert", parameter, commandType: CommandType.StoredProcedure);
                int? newTransId = parameter.Get<int?>("@NewTransId");
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
