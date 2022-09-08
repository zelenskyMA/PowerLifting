using PowerLifting.Domain.Models.TrainingPlan;

namespace PowerLifting.Domain.Interfaces.TrainingPlan.Application
{
    public interface IPlanExerciseSettingsCommands
    {
        /// <summary>
        /// Create exercise settings for plan exercise
        /// </summary>
        /// <param name="planExerciseId">Plan exercise Id</param>
        /// <returns></returns>
        Task Create(int planExerciseId);

        /// <summary>
        /// Get exercise settings for a single planned exercise by Id
        /// </summary>
        /// <param name="planExerciseId">Planned exercise Id</param>
        /// <returns></returns>
        Task<List<PlanExerciseSettings>> GetAsync(int planExerciseId);

        /// <summary>
        /// Get exercise settings for a list of planned exercises by Id
        /// </summary>
        /// <param name="planExerciseId">List of planned exercise Ids</param>
        /// <returns></returns>
        Task<List<PlanExerciseSettings>> GetAsync(List<int> planExerciseIds);
    }
}
