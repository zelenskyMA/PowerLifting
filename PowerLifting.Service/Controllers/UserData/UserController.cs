using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PowerLifting.Application.UserData.UserCommands;
using PowerLifting.Application.UserData.UserCommands.UserCommands;
using PowerLifting.Domain.Interfaces.Common.Operations;
using PowerLifting.Domain.Models.UserData.Auth;

namespace PowerLifting.Service.Controllers.UserData
{
    [ApiController]
    [Route("user")]
    public class UserController : ControllerBase
    {
        [HttpPost]
        [Route("login")]
        public async Task<TokenModel> LoginAsync([FromServices] ICommand<UserLoginCommand.Param, TokenModel> command, UserLoginCommand.Param loginModel)
        {
            var result = await command.ExecuteAsync(loginModel);
            return result;
        }

        [HttpPost]
        [Route("register")]
        public async Task<TokenModel> RegisterAsync([FromServices] ICommand<UserRegisterCommand.Param, TokenModel> command, UserRegisterCommand.Param registrationModel)
        {
            var result = await command.ExecuteAsync(registrationModel);
            return result;
        }

        [HttpPost]
        [Route("changePassword")]
        public async Task<bool> ChangePasswordAsync([FromServices] ICommand<UserChangePasswordCommand.Param, bool> command, UserChangePasswordCommand.Param registrationModel)
        {
            var result = await command.ExecuteAsync(registrationModel);
            return result;
        }

        [HttpGet, Authorize]
        [Route("refreshToken")]
        public async Task<TokenModel> RefreshTokenAsync([FromServices] ICommand<UserRefreshTokenCommand.Param, TokenModel> command)
        {
            var result = await command.ExecuteAsync(new UserRefreshTokenCommand.Param());
            return result;
        }

    }
}