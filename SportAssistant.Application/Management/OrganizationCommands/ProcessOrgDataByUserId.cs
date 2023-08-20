using SportAssistant.Application.UserData.Auth.Interfaces;
using SportAssistant.Domain.DbModels.Management;
using SportAssistant.Domain.Interfaces.Common.Repositories;
using SportAssistant.Domain.Interfaces.Management;
using SportAssistant.Domain.Models.Management;

namespace SportAssistant.Application.Management.OrganizationCommands;

public class ProcessOrgDataByUserId : IProcessOrgDataByUserId
{
    private readonly IProcessManager _processManager;
    private readonly ICrudRepo<OrganizationDb> _orgRepository;
    private readonly IUserProvider _user;

    public ProcessOrgDataByUserId(
        IProcessManager processManager,
        ICrudRepo<OrganizationDb> orgRepository,
        IUserProvider user)
    {
        _processManager = processManager;
        _orgRepository = orgRepository;
        _user = user;
    }

    public async Task<OrganizationDb?> GetOrgByUserIdAsync()
    {
        var orgDb = await _orgRepository.FindOneAsync(t => t.OwnerId == _user.Id);
        return orgDb;
    }

    public async Task<OrgInfo> GetOrgInfoAsync(int orgId, int maxCoaches)
    {
        var managers = await _processManager.GetListAsync(orgId);

        var orgInfo = new OrgInfo()
        {
            ManagerCount = managers.Count,
            DistributedLicences = managers.Sum(t => t.AllowedCoaches),
            LeftToDistribute = maxCoaches - managers.Sum(t => t.AllowedCoaches),
            UsedLicenses = managers.Sum(t => t.DistributedCoaches),
        };

        return orgInfo;
    }
}
