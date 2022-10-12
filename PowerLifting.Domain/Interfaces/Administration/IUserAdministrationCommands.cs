using PowerLifting.Domain.Models.UserData;

namespace PowerLifting.Domain.Interfaces.Administration
{
    public interface IUserAdministrationCommands
    {
        /// <summary>
        /// Apply role changes for selected user
        /// </summary>
        /// <param name="rolesInfo"><see cref="RolesInfo"/>></param>
        /// <returns></returns>
        Task ApplyRolesAsync(RolesInfo rolesInfo);

        /// <summary>
        /// Apply block changes for selected user
        /// </summary>
        /// <param name="blockInfo"><see cref="BlockInfo"/>></param>
        /// <returns></returns>
        Task ApplyBlock(BlockInfo blockInfo);
    }
}
