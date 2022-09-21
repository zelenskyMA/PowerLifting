using PowerLifting.Domain.Models.UserData;

namespace PowerLifting.Domain.Interfaces.Administration
{
    public interface IUserAdministrationCommands
    {
        /// <summary>
        /// Get user information for  administration console
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="login"></param>
        /// <returns></returns>
        Task<UserCard> GetUserCardAsync(int userId, string? login);

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
