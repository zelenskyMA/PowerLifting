using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportAssistant.Application.UserData.UserCommands;
using SportAssistant.Application.UserData.UserCommands.UserCommands;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Models.UserData.Auth;

namespace SportAssistant.Service.Controllers.UserData
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

        [HttpPost]
        [Route("resetPassword")]
        public async Task<bool> ResetPasswordAsync([FromServices] ICommand<UserResetPasswordCommand.Param, bool> command, UserResetPasswordCommand.Param registrationModel)
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