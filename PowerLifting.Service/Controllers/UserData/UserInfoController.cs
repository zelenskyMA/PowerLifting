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
        public async Task<UserInfo> GetAsync()
        {
            var result = await _userInfoCommands.GetAsync();
            return result;
        }

        [HttpGet]
        [Route("getCard")]
        public async Task<UserCard> GetCardAsync(int userId)
        {
            var result = await _userInfoCommands.GetUserCardAsync(userId, null);
            return result;
        }

        [HttpPost]
        [Route("update")]
        public async Task<bool> UpdateAsync(UserInfo userInfo)
        {
            await _userInfoCommands.UpdateAsync(userInfo);
            return true;
        }        
    }
}
