using FinTract_REST_API.Encryption;
using FinTract_REST_API.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace FinTract_REST_API
{
    public class GetUserid : IMiddleware
    {
        private readonly IDbConnection connection;
        private readonly IConfiguration config;
        private readonly ILogger<GetUserid> logger;
        private readonly UserCreds creds;
        public GetUserid(IConfiguration config, ILogger<GetUserid> logger, UserCreds creds)
        {
            this.config = config;
            this.logger = logger;
            connection = new SqlConnection(config.GetConnectionString("DefaultConnection"));
            this.creds = creds;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (context.Request.Path.StartsWithSegments("/api/User/login") && context.Request.Method.ToUpperInvariant() == "POST")
            {
                await next(context);
                return;
            }
            logger.LogInformation(context.Request.Path);
            string bearerToken = context.Request.Headers["Authorization"];
            string token = bearerToken?.Split(" ")[1];
            creds.userid = PasswordHasher.DecodeToken(token);
            await next(context);
            return;
        }
    }
}
