using AutoMapper;
using SportAssistant.Domain.CustomExceptions;
using SportAssistant.Domain.DbModels.Management;
using SportAssistant.Domain.Enums;
using SportAssistant.Domain.Interfaces.Common.Repositories;
using SportAssistant.Domain.Interfaces.Management;
using SportAssistant.Domain.Interfaces.UserData.Application;
using SportAssistant.Domain.Models.Management;

namespace SportAssistant.Application.Management.ManagerCommands;

public class ProcessManager : IProcessManager
{
    private readonly IUserRoleCommands _userRoleCommands;
    private readonly IProcessCoachAssignment _processCoachAssignment;
    private readonly ICrudRepo<ManagerDb> _managerRepository;
    private readonly IMapper _mapper;

    public ProcessManager(
        IUserRoleCommands userRoleCommands,
        IProcessCoachAssignment processCoachAssignment,
        ICrudRepo<ManagerDb> managerRepository,
        IMapper mapper)
    {
        _userRoleCommands = userRoleCommands;
        _processCoachAssignment = processCoachAssignment;
        _managerRepository = managerRepository;
        _mapper = mapper;
    }

    public async Task<Manager> GetBaseAsync(int managerId)
    {
        var managerDb = await _managerRepository.FindOneAsync(t => t.UserId == managerId);
        if (managerDb == null)
        {
            throw new BusinessException("Учетка менеджера не найдена");
        }

        var manager = _mapper.Map<Manager>(managerDb);
        var info = await _processCoachAssignment.GetAssignedCoachesAsync(new List<int>() { managerId });

        manager.DistributedCoaches = info[0].CoachIds.Count;

        return manager;
    }

    /// <inheritdoc />
    public async Task<List<Manager>> GetListAsync(int orgId)
    {
        if (!await _userRoleCommands.IHaveAnyRoles(new[] { UserRoles.Admin, UserRoles.OrgOwner, UserRoles.Manager }))
        {
            throw new RoleException();
        }

        var managersDb = await _managerRepository.FindAsync(t => t.OrgId == orgId);
        var managers = managersDb.Select(_mapper.Map<Manager>).ToList();

        var coaches = await _processCoachAssignment.GetAssignedCoachesAsync(managers.Select(t => t.Id).ToList());
        foreach (var item in managers)
        {
            item.DistributedCoaches = coaches.FirstOrDefault(t => t.ManagerId == item.Id)?.CoachIds?.Count ?? 0;
        }

        return managers;
    }
}
