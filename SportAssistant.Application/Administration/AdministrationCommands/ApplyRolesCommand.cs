using SportAssistant.Domain.Enums;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.UserData.Application;
using SportAssistant.Domain.Models.UserData;

namespace SportAssistant.Application.Administration.AdministrationCommands;

/// <summary>
/// Изменение ролей для выбранного пользователя. Добавляем и удаляем.
/// </summary>
public class ApplyRolesCommand : ICommand<ApplyRolesCommand.Param, bool>
{
    private readonly IUserRoleCommands _userRoleCommands;

    public ApplyRolesCommand(IUserRoleCommands userRoleCommands)
    {
        _userRoleCommands = userRoleCommands;
    }

    /// <inheritdoc />
    public async Task<bool> ExecuteAsync(Param param)
    {
        await UpdateRole(param.UserId, param.IsAdmin, UserRoles.Admin);
        await UpdateRole(param.UserId, param.IsCoach, UserRoles.Coach);
        await UpdateRole(param.UserId, param.IsManager, UserRoles.Manager);
        await UpdateRole(param.UserId, param.IsOrgOwner, UserRoles.OrgOwner);

        return true;
    }

    private async Task UpdateRole(int userId, bool hasRole, UserRoles role)
    {
        if (hasRole)
        {
            await _userRoleCommands.AddRole(userId, role);
        }
        else
        {
            await _userRoleCommands.RemoveRole(userId, role);
        }
    }

    public class Param : RolesInfo { }
}
