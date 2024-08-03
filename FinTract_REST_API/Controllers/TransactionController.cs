using FinTract_REST_API.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinTract_REST_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionServices services;
        public TransactionController(ITransactionServices services)
        {
            this.services = services;
        }

        [HttpPost]
        public async Task<IActionResult> TransactionProcess(int userid, int amount, int accountid)
        {
            var response = await services.ProcessTransaction(userid, amount, accountid);
            if (response)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetHistory(string userid)
        {
            var result = await services.GetHistory(userid);
            if(result == null)
            {
                return BadRequest("no data");
            }
            return Ok(result);
        }
    }
}
