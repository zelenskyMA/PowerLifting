using SportAssistant.Application.UserData.Auth.Interfaces;
using SportAssistant.Domain.CustomExceptions;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.Management;
using SportAssistant.Domain.Interfaces.UserData.Application;
using SportAssistant.Domain.Models.Management;

namespace SportAssistant.Application.Management.ManagerCommands;

public class ManagerGetListQuery : ICommand<ManagerGetListQuery.Param, List<Manager>>
{
    private readonly IProcessManager _processManager;
    private readonly IProcessUserInfo _processUserInfo;
    private readonly IProcessOrgData _processOrgData;

    public ManagerGetListQuery(
        IProcessManager processManager,
        IProcessUserInfo processUserInfo,
        IProcessOrgData processOrgData,
        IUserProvider user)
    {
        _processManager = processManager;
        _processUserInfo = processUserInfo;
        _processOrgData = processOrgData;
    }

    /// <inheritdoc />
    public async Task<List<Manager>> ExecuteAsync(Param param)
    {
        var org = await _processOrgData.GetOrgByUserIdAsync() ?? await _processOrgData.GetOrgByManagerIdAsync();
        if (org == null)
        {
            throw new RoleException();
        }

        var managerList = await _processManager.GetListAsync(org.Id);

        var infoList = await _processUserInfo.GetInfoList(managerList.Select(t => t.Id).ToList());
        foreach (var manager in managerList)
        {
            manager.Name = infoList.FirstOrDefault(t => t.Id == manager.Id)?.LegalName ?? string.Empty;
        }

        return managerList;
    }

    public class Param
    {
    }
}
