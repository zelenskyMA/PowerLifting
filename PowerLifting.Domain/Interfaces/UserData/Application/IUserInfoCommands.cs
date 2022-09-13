using PowerLifting.Domain.Models.UserData;

namespace PowerLifting.Domain.Interfaces.UserData.Application
{
    public interface IUserInfoCommands
    {
        /// <summary>
        /// Get user information by userId.
        /// </summary>
        /// <returns>user information</returns>
        Task<UserInfo> GetAsync();

        /// <summary>
        /// Update user information
        /// </summary>
        /// <param name="userInfo">User information</param>
        /// <returns></returns>
        Task UpdateAsync(UserInfo userInfo);
    }
}
