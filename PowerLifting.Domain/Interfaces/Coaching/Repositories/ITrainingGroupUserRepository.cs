using PowerLifting.Domain.DbModels.Coaching;
using PowerLifting.Domain.DbModels.UserData;
using PowerLifting.Domain.Interfaces.Common.Repositories;

namespace PowerLifting.Domain.Interfaces.Coaching.Repositories
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
