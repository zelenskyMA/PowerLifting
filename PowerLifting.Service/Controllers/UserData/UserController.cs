using Microsoft.AspNetCore.Authorization;
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
        public async Task<TokenModel> LoginAsync([FromBody] LoginModel loginModel)
        {
            var result = await _userCommands.LoginAsync(loginModel);
            return result;
        }

        [HttpPost]
        [Route("register")]
        public async Task<TokenModel> RegisterAsync([FromBody] RegistrationModel registrationModel)
        {
            var result = await _userCommands.RegisterAsync(registrationModel);
            return result;
        }

        [HttpPost]
        [Route("changePassword")]
        public async Task<bool> ChangePasswordAsync([FromBody] RegistrationModel registrationModel)
        {
            await _userCommands.ChangePasswordAsync(registrationModel);
            return true;
        }

        [HttpGet, Authorize]
        [Route("refreshToken")]
        public async Task<TokenModel> RefreshTokenAsync()
        {
            var result = await _userCommands.RefreshTokenAsync();
            return result;
        }

    }
}
