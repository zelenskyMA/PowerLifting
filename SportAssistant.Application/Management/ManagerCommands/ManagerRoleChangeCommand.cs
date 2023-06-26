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
    private readonly ICrudRepo<ManagerDb> _managerRepository;

    public ManagerRoleChangeCommand(
        IUserRoleCommands userRoleCommands,
        IProcessCoachAssignment processCoachAssignment,
        ICrudRepo<ManagerDb> managerRepository)
    {
        _userRoleCommands = userRoleCommands;
        _processCoachAssignment = processCoachAssignment;
        _managerRepository = managerRepository;
    }

    /// <inheritdoc />
    public async Task<bool> ExecuteAsync(Param param)
    {
        if (!await _userRoleCommands.IHaveRole(UserRoles.OrgOwner))
        {
            throw new RoleException();
        }

        return param.RoleStatus ? await AddManagerStatus(param) : await RemoveManagerStatus(param);
    }

    private async Task<bool> AddManagerStatus(Param param)
    {
        await _userRoleCommands.AddRole(param.UserId, UserRoles.Manager);

        await _managerRepository.CreateAsync(new ManagerDb()
        {
            UserId = param.UserId,
        });

        return true;
    }

    private async Task<bool> RemoveManagerStatus(Param param)
    {
        await _processCoachAssignment.DropCoachesByManagerAsync(param.UserId);

        await _userRoleCommands.RemoveRole(param.UserId, UserRoles.Manager);

        var managerInfo = await _managerRepository.FindOneAsync(t => t.UserId == param.UserId);
        if (managerInfo == null)
        {
            return false;
        }

        _managerRepository.Delete(managerInfo);

        return true;
    }

    public class Param
    {
        public int UserId { get; set; }

        public bool RoleStatus { get; set; }
    }
}
