using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AccountService _accountService;
        public AccountController(AccountService accountService){
            _accountService = accountService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Create([FromBody] UserRequest request)
        {
            var result = await _accountService.Create(request);
            if(!result.Succeeded){
                return BadRequest(JsonSerializer.Serialize(result.Errors));
            }
            return NoContent();
        }

        [HttpPost("Delete")]
        public async Task<IActionResult> Delete([FromBody] RemoveUserRequest request)
        {
            await _accountService.Delete(request.Username, request.Provider);
            return NoContent();
        }
    }
}