using PowerLifting.Domain.Models.UserData;

namespace PowerLifting.Domain.Interfaces.UserData.Application
{
    public interface IUserBlockCommands
    {
        /// <summary>
        /// Get user block reason
        /// </summary>
        /// <param name="userId">Blocked user Id</param>
        /// <returns></returns>
        Task<UserBlockHistory> GetCurrentBlockReason(int userId);

        /// <summary>
        /// Block user
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <param name="reason">Block reason</param>
        /// <returns></returns>
        Task BlockUser(int userId, string reason);

        /// <summary>
        /// Remove user block
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns></returns>
        Task UnblockUser(int userId);
    }
}
