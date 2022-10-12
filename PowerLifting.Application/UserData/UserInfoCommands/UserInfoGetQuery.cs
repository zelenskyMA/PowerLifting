using PowerLifting.Application.UserData.Auth.Interfaces;
using PowerLifting.Domain.Interfaces.Common.Actions;
using PowerLifting.Domain.Interfaces.UserData.Application;
using PowerLifting.Domain.Models.UserData;

namespace PowerLifting.Application.UserData.UserInfoCommands
{
    public class UserInfoGetQuery : ICommand<UserInfoGetQuery.Param, UserInfo>
    {
        private readonly IProcessUserInfo _processUserInfo;
        private readonly IUserProvider _user;
        public UserInfoGetQuery(
            IProcessUserInfo processUserInfo,
            IUserProvider user)
        {
            _processUserInfo = processUserInfo;
            _user = user;
        }

        /// <inheritdoc />
        public async Task<UserInfo> ExecuteAsync(Param param)
        {
            var info = await _processUserInfo.GetInfo(_user.Id);
            return info;
        }

        public class Param { }
    }
}
