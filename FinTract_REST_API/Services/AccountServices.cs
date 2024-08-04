using Dapper;
using FinTract_REST_API.Encryption;
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
        private readonly UserCreds creds;
        public AccountServices(IConfiguration config, ILogger<AccountServices> logger, UserCreds creds)
        {
            this.config = config;
            this.connection = new SqlConnection(config.GetConnectionString("DefaultConnection"));
            this.creds = creds;
        }
        public async Task<bool> CreateAccount(Accounts account)
        {
            try
            {
                var userid = creds.userid;
                var parameter = new DynamicParameters();
                parameter.Add("@TransactionName", account.TransactionName);
                parameter.Add("@UserId", userid);
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

        public async Task<IEnumerable<Accounts>> GetAllAccount()
        {
            try
            {
                var userid = creds.userid;
                var parameter = new DynamicParameters();
                parameter.Add("@UserId", userid);

                var result = await connection.QueryAsync<Accounts>("Get_Accounts", parameter, commandType: CommandType.StoredProcedure);
                return result;
            }
            catch(Exception ex)
            {
                logger.LogError(ex.Message);
                return null;
            }
        }
    }
}
