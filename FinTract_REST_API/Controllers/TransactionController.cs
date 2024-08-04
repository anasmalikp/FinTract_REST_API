using FinTract_REST_API.Interfaces;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        public async Task<IActionResult> TransactionProcess(int amount, int accountid)
        {
            var response = await services.ProcessTransaction(amount, accountid);
            if (response)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetHistory()
        {
            var result = await services.GetHistory();
            if(result == null)
            {
                return BadRequest("no data");
            }
            return Ok(result);
        }
    }
}
