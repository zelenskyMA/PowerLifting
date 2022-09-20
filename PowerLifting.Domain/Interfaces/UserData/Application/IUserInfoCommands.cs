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

        /// <summary>
        /// Get user card by admin / trainer / selt
        /// </summary>
        /// <param name="userId">Find user by id</param>
        /// <param name="login">Find user by login</param>
        /// <returns>User card</returns>
        Task<UserCard> GetUserCardAsync(int userId, string login);
    }
}
