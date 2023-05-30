using SportAssistant.Domain.Enums;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.UserData.Application;

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
        if (param.IsAdmin)
        {
            await _userRoleCommands.AddRole(param.UserId, UserRoles.Admin);
        }
        else
        {
            await _userRoleCommands.RemoveRole(param.UserId, UserRoles.Admin);
        }

        if (param.IsCoach)
        {
            await _userRoleCommands.AddRole(param.UserId, UserRoles.Coach);
        }
        else
        {
            await _userRoleCommands.RemoveRole(param.UserId, UserRoles.Coach);
        }

        return true;
    }

    public class Param
    {
        /// <summary>
        /// UI supplies this field for role update
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// True if user har administration role
        /// </summary>
        public bool IsAdmin { get; set; } = false;

        /// <summary>
        /// True if user har coach role
        /// </summary>
        public bool IsCoach { get; set; } = false;
    }
}
