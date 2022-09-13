using Microsoft.AspNetCore.Mvc;
using PowerLifting.Domain.Interfaces.UserData.Application;
using PowerLifting.Domain.Models.UserData;

namespace PowerLifting.Service.Controllers.UserData
{
    [Route("userInfo")]
    public class UserInfoController : BaseController
    {
        private readonly IUserInfoCommands _userInfoCommands;

        public UserInfoController(IUserInfoCommands userInfoCommands)
        {
            _userInfoCommands = userInfoCommands;
        }

        [HttpGet]
        [Route("get")]
        public async Task<UserInfo> Get()
        {
            var result = await _userInfoCommands.GetAsync();
            return result;
        }
    }
}
