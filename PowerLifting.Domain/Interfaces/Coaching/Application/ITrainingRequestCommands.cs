using PowerLifting.Domain.Models.Coaching;

namespace PowerLifting.Domain.Interfaces.Coaching.Application
{
    public interface ITrainingRequestCommands
    {
        /// <summary>
        /// Chech my request
        /// </summary>
        /// <returns></returns>
        Task<TrainingRequest> GetMyRequestAsync();

        /// <summary>
        /// Chech user request. For internal use only
        /// </summary>
        /// <returns></returns>
        Task<TrainingRequest> GetUserRequestAsync(int userId);

        /// <summary>
        /// Get user requests for coach
        /// </summary>
        /// <returns></returns>
        Task<List<TrainingRequest>> GetCoachRequestsAsync();

        /// <summary>
        /// Get user request bu requestId
        /// </summary>
        /// <param name="id">request Id</param>
        /// <returns></returns>
        Task<TrainingRequest> GetCoachRequestAsync(int id);

        /// <summary>
        /// Get active coaches for user selection
        /// </summary>
        /// <returns></returns>
        Task<List<CoachInfo>> GetCoachesAsync();

        /// <summary>
        /// Create request for coach
        /// </summary>
        /// <param name="coachId">Coach Id</param>
        /// <returns></returns>
        Task CreateAsync(int coachId);

        /// <summary>
        /// Remove my request, or who's request coach wants to remove
        /// </summary>
        /// <param name="userId">user Id</param>
        /// <returns></returns>
        Task RemoveRequestAsync(int userId);
    }
}
