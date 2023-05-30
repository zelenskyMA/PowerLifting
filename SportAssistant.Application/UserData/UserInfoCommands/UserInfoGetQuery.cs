using SportAssistant.Application.UserData.Auth.Interfaces;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.UserData.Application;
using SportAssistant.Domain.Models.UserData;

namespace SportAssistant.Application.UserData.UserInfoCommands;

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
