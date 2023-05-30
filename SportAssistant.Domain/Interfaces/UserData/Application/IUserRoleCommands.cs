using SportAssistant.Domain.Enums;
using SportAssistant.Domain.Models;
using SportAssistant.Domain.Models.UserData;

namespace SportAssistant.Domain.Interfaces.UserData.Application;

public interface IUserRoleCommands
{
    /// <summary>
    /// Get roles, existing in application
    /// </summary>
    /// <returns>Dictionary items with type UseRole</returns>
    Task<List<DictionaryItem>> GetRolesList();

    /// <summary>
    /// Get selected user roles
    /// </summary>
    /// <param name="userId">User Id</param>
    /// <returns>List of roles</returns>
    Task<RolesInfo> GetUserRoles(int userId);

    /// <summary>
    /// Check executor role. 
    /// </summary>
    /// <param name="role">Role id to check</param>
    /// <returns></returns>
    Task<bool> IHaveRole(UserRoles role);

    /// <summary>
    /// Add role for selected user
    /// </summary>
    /// <param name="userId">User Id</param>
    /// <param name="role">Role Id</param>
    /// <returns></returns>
    Task AddRole(int userId, UserRoles role);

    /// <summary>
    /// Remove role for selected user
    /// </summary>
    /// <param name="userId">User Id</param>
    /// <param name="role">Role Id</param>
    /// <returns></returns>
    Task RemoveRole(int userId, UserRoles role);
}
