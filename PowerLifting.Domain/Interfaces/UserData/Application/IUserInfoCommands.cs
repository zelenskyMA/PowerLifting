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
    }
}
