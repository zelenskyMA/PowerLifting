using SportAssistant.Domain.DbModels.Coaching;
using SportAssistant.Domain.DbModels.UserData;
using SportAssistant.Domain.Interfaces.Common.Repositories;

namespace SportAssistant.Domain.Interfaces.Coaching.Repositories
{
    public interface ITrainingGroupUserRepository : ICrudRepo<TrainingGroupUserDb>
    {
        /// <summary>
        /// Get list of users in selected group
        /// </summary>
        /// <param name="groupId">Group Id</param>
        /// <returns></returns>
        Task<List<UserInfoDb>> GetGroupUsersAsync(int groupId);
    }
}
