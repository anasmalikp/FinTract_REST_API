using Dapper;
using FinTract_REST_API.Interfaces;
using FinTract_REST_API.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace FinTract_REST_API.Services
{
    public class TransactionServices:ITransactionServices
    {
        private readonly IDbConnection connection;
        private readonly IConfiguration config;
        private readonly ILogger<TransactionServices> logger;
        public TransactionServices(IConfiguration config, ILogger<TransactionServices> logger)
        {
            this.config = config;
            this.connection = new SqlConnection(config.GetConnectionString("DefaultConnection"));
            this.logger = logger;
        }

        public async Task<bool> ProcessTransaction (int userid, int amount, int accountid)
        {
            try
            {
                var parameter = new DynamicParameters();
                parameter.Add("@Userid", userid);
                parameter.Add("@Amt", amount);
                parameter.Add("@AccountId", accountid);
                parameter.Add("@Errormsg", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

                await connection.ExecuteAsync("process_transaction", parameter, commandType: CommandType.StoredProcedure);

                string? Errormsg = parameter.Get<string?>("@Errormsg");
                logger.LogError(Errormsg);

                return true;
            }
            catch(Exception ex)
            {
                logger.LogError(ex.Message);
                return false;
            }
        }

        public async Task<IEnumerable<History>> GetHistory(string userid)
        {
            try
            {
                var parameter = new DynamicParameters();
                parameter.Add("@Userid", userid);

                var result = await connection.QueryAsync<History>("Get_History", parameter, commandType: CommandType.StoredProcedure);

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
