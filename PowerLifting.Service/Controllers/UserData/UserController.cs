using Microsoft.AspNetCore.Mvc;
using PowerLifting.Domain.Interfaces.UserData.Application;
using PowerLifting.Domain.Models.Auth;
using PowerLifting.Domain.Models.UserData.Auth;

namespace PowerLifting.Service.Controllers.UserData
{
    [ApiController]
    [Route("user")]
    public class UserController : ControllerBase
    {
        private readonly IUserCommands _userCommands;

        public UserController(IUserCommands userCommands)
        {
            _userCommands = userCommands;
        }

        [HttpPost]
        [Route("login")]
        public async Task<string> LoginAsync([FromBody] LoginModel loginModel)
        {
            var result = await _userCommands.LoginAsync(loginModel);
            return result;
        }

        [HttpPost]
        [Route("register")]
        public async Task<string> RegisterAsync([FromBody] RegistrationModel registrationModel)
        {
            var result = await _userCommands.RegisterAsync(registrationModel);
            return result;
        }

        [HttpPost]
        [Route("changePassword")]
        public async Task<string> ChangePasswordAsync([FromBody] RegistrationModel registrationModel)
        {
            var result = await _userCommands.ChangePasswordAsync(registrationModel);
            return result;
        }
    }
}
