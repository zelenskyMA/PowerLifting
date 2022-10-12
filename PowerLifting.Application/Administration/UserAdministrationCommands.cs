using PowerLifting.Domain.Enums;
using PowerLifting.Domain.Interfaces.Administration;
using PowerLifting.Domain.Interfaces.UserData.Application;
using PowerLifting.Domain.Models.UserData;

namespace PowerLifting.Application.Administration
{
    public class UserAdministrationCommands : IUserAdministrationCommands
    {
        private readonly IUserRoleCommands _userRoleCommands;
        private readonly IUserBlockCommands _userBlockCommands;

        public UserAdministrationCommands(
            IUserRoleCommands userRoleCommands,
            IUserBlockCommands userBlockCommands)
        {
            _userRoleCommands = userRoleCommands;
            _userBlockCommands = userBlockCommands;
        }

        public async Task ApplyRolesAsync(RolesInfo rolesInfo)
        {
            if (rolesInfo.IsAdmin)
            {
                await _userRoleCommands.AddRole(rolesInfo.UserId, UserRoles.Admin);
            }
            else
            {
                await _userRoleCommands.RemoveRole(rolesInfo.UserId, UserRoles.Admin);
            }

            if (rolesInfo.IsCoach)
            {
                await _userRoleCommands.AddRole(rolesInfo.UserId, UserRoles.Coach);
            }
            else
            {
                await _userRoleCommands.RemoveRole(rolesInfo.UserId, UserRoles.Coach);
            }
        }

        public async Task ApplyBlock(BlockInfo blockInfo)
        {
            if (blockInfo.Status)
            {
                await _userBlockCommands.BlockUser(blockInfo.UserId, blockInfo.Reason);
            }
            else
            {
                await _userBlockCommands.UnblockUser(blockInfo.UserId);
            }
        }

    }
}
