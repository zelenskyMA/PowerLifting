using SportAssistant.Domain.CustomExceptions;
using SportAssistant.Domain.DbModels.Management;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.Common.Repositories;
using SportAssistant.Domain.Interfaces.Management;

namespace SportAssistant.Application.Management.ManagerCommands;

public class ManagerTrnasferCommand : ICommand<ManagerTrnasferCommand.Param, bool>
{
    private readonly IProcessCoachAssignment _processCoachAssignment;
    private readonly IProcessOrgData _processOrgData;
    private readonly ICrudRepo<ManagerDb> _managerRepository;

    public ManagerTrnasferCommand(
        IProcessCoachAssignment processCoachAssignment,
        IProcessOrgData processOrgData,
        ICrudRepo<ManagerDb> managerRepository)
    {
        _processCoachAssignment = processCoachAssignment;
        _processOrgData = processOrgData;
        _managerRepository = managerRepository;
    }

    /// <inheritdoc />
    public async Task<bool> ExecuteAsync(Param param)
    {
        (ManagerDb source, ManagerDb target) = await GetValidatedManagers(param);

        var reassignedCoachCount = await _processCoachAssignment.ReassignCoachesAsync(source.UserId, target.UserId, param.CoachIds);

        target.AllowedCoaches += reassignedCoachCount == 0 ? source.AllowedCoaches : reassignedCoachCount;
        source.AllowedCoaches = reassignedCoachCount == 0 ? 0 : (source.AllowedCoaches - reassignedCoachCount);

        _managerRepository.UpdateList(new List<ManagerDb>() { source, target });

        return true;
    }

    private async Task<(ManagerDb source, ManagerDb target)> GetValidatedManagers(Param param)
    {
        var org = await _processOrgData.GetOrgByUserIdAsync() ?? await _processOrgData.GetOrgByManagerIdAsync();
        if (org == null)
        {
            throw new RoleException();
        }

        var source = await _managerRepository.FindOneAsync(t => t.UserId == param.SourceManagerId);
        var target = await _managerRepository.FindOneAsync(t => t.UserId == param.TargetManagerId);
        if (source?.OrgId != org.Id)
        {
            throw new BusinessException("Выбранный менеджер не принадлежит вашей организации");
        }

        if (target?.OrgId != org.Id)
        {
            throw new BusinessException("Целевой менеджер не принадлежит вашей организации");
        }

        return (source, target);
    }


    public class Param
    {
        public int SourceManagerId { get; set; }

        public int TargetManagerId { get; set; }

        public List<int> CoachIds { get; set; } = new List<int>();
    }
}
