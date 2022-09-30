using PowerLifting.Domain.DbModels.Coaching;
using PowerLifting.Domain.DbModels.UserData;
using PowerLifting.Domain.Interfaces.Common.Repositories;

namespace PowerLifting.Domain.Interfaces.Coaching.Repositories
{
    public interface ITrainingRequestRepository : ICrudRepo<TrainingRequestDb>
    {
        /// <summary>
        /// Get list of active coaches
        /// </summary>
        /// <returns></returns>
        Task<List<UserInfoDb>> GetCoachesAsync();

        /// <summary>
        /// Get list of active users from training requests
        /// </summary>
        /// <param name="requestedUserIds">List of user Id's</param>
        /// <returns></returns>
        Task<List<UserInfoDb>> GetUsersAsync(List<int> requestedUserIds);
    }
}
