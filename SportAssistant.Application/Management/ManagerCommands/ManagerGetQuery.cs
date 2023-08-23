using SportAssistant.Application.UserData.Auth.Interfaces;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.Management;
using SportAssistant.Domain.Interfaces.UserData.Application;
using SportAssistant.Domain.Models.Management;

namespace SportAssistant.Application.Management.ManagerCommands;

public class ManagerGetQuery : ICommand<ManagerGetQuery.Param, Manager>
{
    private readonly IProcessUserInfo _processUserInfo;
    private readonly IProcessManager _processManager;
    private readonly IUserProvider _user;

    public ManagerGetQuery(
        IProcessUserInfo processUserInfo,
        IProcessManager processManager,
        IUserProvider user)
    {
        _processUserInfo = processUserInfo;
        _processManager = processManager;
        _user = user;
    }

    /// <inheritdoc />
    public async Task<Manager> ExecuteAsync(Param param)
    {
        param.Id = param.Id == 0 ? _user.Id : param.Id;

        var manager = await _processManager.GetBaseAsync(param.Id);

        var info = await _processUserInfo.GetInfoList(new List<int>() { manager.Id });
        manager.Name = info.FirstOrDefault()?.LegalName ?? string.Empty;

        return manager;
    }

    public class Param
    {
        public int Id { get; set; }
    }
}
