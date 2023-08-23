using SportAssistant.Domain.CustomExceptions;
using SportAssistant.Domain.DbModels.Management;
using SportAssistant.Domain.Enums;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.Common.Repositories;
using SportAssistant.Domain.Interfaces.Management;
using SportAssistant.Domain.Interfaces.UserData.Application;

namespace SportAssistant.Application.Management.ManagerCommands;

public class ManagerRoleChangeCommand : ICommand<ManagerRoleChangeCommand.Param, bool>
{
    private readonly IUserRoleCommands _userRoleCommands;
    private readonly IProcessCoachAssignment _processCoachAssignment;
    private readonly IProcessOrgData _processOrgData;
    private readonly ICrudRepo<ManagerDb> _managerRepository;

    public ManagerRoleChangeCommand(
        IUserRoleCommands userRoleCommands,
        IProcessCoachAssignment processCoachAssignment,
        IProcessOrgData processOrgData,
        ICrudRepo<ManagerDb> managerRepository)
    {
        _userRoleCommands = userRoleCommands;
        _processCoachAssignment = processCoachAssignment;
        _processOrgData = processOrgData;
        _managerRepository = managerRepository;
    }

    /// <inheritdoc />
    public async Task<bool> ExecuteAsync(Param param)
    {
        var org = await _processOrgData.GetOrgByUserIdAsync() ?? await _processOrgData.GetOrgByManagerIdAsync();
        if (org == null)
        {
            throw new RoleException();
        }

        return param.RoleStatus ?
            await AddManagerStatus(param.UserId, org) :
            await RemoveManagerStatus(param.UserId, org);
    }

    private async Task<bool> AddManagerStatus(int userId, OrganizationDb org)
    {
        var managerInfo = await _managerRepository.FindOneAsync(t => t.UserId == userId);
        if (managerInfo != null)
        {
            throw new BusinessException("Указанный пользователь уже менеджер"); // возможно, менеджер другой организации
        }

        await _userRoleCommands.AddRole(userId, UserRoles.Manager);

        await _managerRepository.CreateAsync(new ManagerDb()
        {
            UserId = userId,
            OrgId = org.Id,
        });

        return true;
    }

    private async Task<bool> RemoveManagerStatus(int userId, OrganizationDb org)
    {
        var managerInfo = await _managerRepository.FindOneAsync(t => t.UserId == userId);
        if (managerInfo == null)
        {
            throw new BusinessException("Указанный пользователь не менеджер");
        }

        if (managerInfo.OrgId != org.Id)
        {
            throw new BusinessException("Указанный менеджер из другой организации");
        }

        await _processCoachAssignment.DropCoachesByManagerAsync(userId);

        await _userRoleCommands.RemoveRole(userId, UserRoles.Manager);

        _managerRepository.Delete(managerInfo);

        return true;
    }

    public class Param
    {
        public int UserId { get; set; }

        public bool RoleStatus { get; set; }
    }
}
