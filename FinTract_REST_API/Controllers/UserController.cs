using FinTract_REST_API.Interfaces;
using FinTract_REST_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinTract_REST_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserServices services;
        public UserController(IUserServices services)
        {
            this.services = services;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser(Users user)
        {
            var result = await services.RegisterUser(user);
            if(result!= -1)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(Users user)
        {
            var result = await services.LoginUser(user);
            if(result == null)
            {
                return BadRequest("something went wrong");
            }
            return Ok(result);
        }
    }
}
