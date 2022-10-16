using SportAssistant.Domain.Models.UserData;

namespace SportAssistant.Domain.Interfaces.UserData.Application
{
    public interface IProcessUserInfo
    {
        /// <summary>
        /// Collect  user info data by user Id
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns></returns>
        Task<UserInfo> GetInfo(int userId);
    }
}
