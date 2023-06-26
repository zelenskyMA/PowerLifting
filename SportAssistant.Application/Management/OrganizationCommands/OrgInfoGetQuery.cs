using SportAssistant.Application.UserData.Auth.Interfaces;
using SportAssistant.Domain.DbModels.Management;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.Common.Repositories;
using SportAssistant.Domain.Interfaces.Management;
using SportAssistant.Domain.Models.Management;

namespace SportAssistant.Application.Management.OrganizationCommands;

/// <summary>
/// Получение дополнительной информации по организации для ее владельца.
/// </summary>
public class OrgInfoGetQuery : ICommand<OrgInfoGetQuery.Param, OrgInfo>
{
    private readonly IProcessManager _processManager;
    private readonly ICrudRepo<OrganizationDb> _orgRepository;
    private readonly IUserProvider _user;

    public OrgInfoGetQuery(
        IProcessManager processManager,
        ICrudRepo<OrganizationDb> orgRepository,
        IUserProvider user)
    {
        _processManager = processManager;
        _orgRepository = orgRepository;
        _user = user;
    }

    public async Task<OrgInfo> ExecuteAsync(Param param)
    {
        var orgDb = await _orgRepository.FindOneAsync(t => t.OwnerId == _user.Id);
        if (orgDb == null)
        {
            return new OrgInfo();
        }

        var managers = await _processManager.GetListAsync(orgDb.Id);

        var orgInfo = new OrgInfo()
        {
            ManagerCount = managers.Count,
            DistributedLicences = managers.Sum(t => t.AllowedCoaches),
            UsedLicenses = managers.Sum(t => t.AssignedCoaches),
        };

        return orgInfo;
    }

    public class Param
    {
    }
}
