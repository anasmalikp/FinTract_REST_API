using FinTract_REST_API.Interfaces;
using FinTract_REST_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinTract_REST_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountServices services;
        public AccountController(IAccountServices services)
        {
            this.services = services;
        }

        [HttpPost]
        public async Task<IActionResult> AddAccount(Accounts account)
        {
            var response = await services.CreateAccount(account);
            if (response)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
