using Microsoft.AspNetCore.Mvc;
using PowerLifting.Application.UserData.UserInfoCommands;
using PowerLifting.Domain.Interfaces.Common.Operations;
using PowerLifting.Domain.Models.UserData;

namespace PowerLifting.Service.Controllers.UserData
{
    [Route("userInfo")]
    public class UserInfoController : BaseController
    {
        [HttpGet]
        [Route("get")]
        public async Task<UserInfo> GetAsync([FromServices] ICommand<UserInfoGetQuery.Param, UserInfo> command)
        {
            var result = await command.ExecuteAsync(new UserInfoGetQuery.Param());
            return result;
        }

        [HttpGet]
        [Route("getCard")]
        public async Task<UserCard> GetCardAsync([FromServices] ICommand<UserInfoGetCardQuery.Param, UserCard> command, int userId)
        {
            var result = await command.ExecuteAsync(new UserInfoGetCardQuery.Param() { UserId = userId });
            return result;
        }

        [HttpPost]
        [Route("update")]
        public async Task<bool> UpdateAsync([FromServices] ICommand<UserInfoUpdateCommand.Param, bool> command, UserInfo userInfo)
        {
            var result = await command.ExecuteAsync(new UserInfoUpdateCommand.Param() { Info = userInfo });
            return result;
        }
    }
}
