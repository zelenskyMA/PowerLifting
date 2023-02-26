using Microsoft.AspNetCore.Mvc;
using SportAssistant.Application.UserData.UserInfoCommands;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Models.UserData;

namespace SportAssistant.Service.Controllers.UserData
{
    [Route("userInfo")]
    public class UserInfoController : BaseController
    {
        [HttpGet]
        public async Task<UserInfo> GetAsync([FromServices] ICommand<UserInfoGetQuery.Param, UserInfo> command)
        {
            var result = await command.ExecuteAsync(new UserInfoGetQuery.Param());
            return result;
        }

        [HttpGet]
        [Route("getCard/{userId}")]
        public async Task<UserCard> GetCardAsync([FromServices] ICommand<UserInfoGetCardQuery.Param, UserCard> command, int userId)
        {
            var result = await command.ExecuteAsync(new UserInfoGetCardQuery.Param() { UserId = userId });
            return result;
        }

        [HttpPost]
        public async Task<bool> UpdateAsync([FromServices] ICommand<UserInfoUpdateCommand.Param, bool> command, UserInfoUpdateCommand.Param param)
        {
            var result = await command.ExecuteAsync(param);
            return result;
        }
    }
}
