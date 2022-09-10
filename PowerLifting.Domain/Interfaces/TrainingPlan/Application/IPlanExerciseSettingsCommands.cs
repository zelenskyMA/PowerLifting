using PowerLifting.Domain.Models.TrainingPlan;

namespace PowerLifting.Domain.Interfaces.TrainingPlan.Application
{
    public interface IPlanExerciseSettingsCommands
    {
        /// <summary>
        /// Get exercise settings for a single planned exercise by Id
        /// </summary>
        /// <param name="planExerciseId">Planned exercise Id</param>
        /// <returns></returns>
        Task<PlanExerciseSettings> GetAsync(int id);

        /// <summary>
        /// Get exercise settings for a list of planned exercises by Id
        /// </summary>
        /// <param name="planExerciseId">List of planned exercise Ids</param>
        /// <returns></returns>
        Task<List<PlanExerciseSettings>> GetAsync(List<int> planExerciseIds);

        /// <summary>
        /// Get full list of current planDay percentages
        /// </summary>
        /// <returns></returns>
        Task<List<Percentage>> GetPercentagesAsync();

        /// <summary>
        /// Create exercise settings for plan exercise
        /// </summary>
        /// <param name="planExerciseId">Plan exercise Id</param>
        /// <returns></returns>
        Task CreateAsync(int planExerciseId);

        /// <summary>
        /// Update exercise settings for plan exercise
        /// </summary>
        /// <param name="settings">Plan exercise settings</param>
        /// <returns></returns>
        Task UpdateAsync(PlanExerciseSettings settings);

        /// <summary>
        /// Delete all supplied items
        /// </summary>
        /// <param name="settings">Settings for deleting</param>
        /// <returns></returns>
        Task DeleteByPlanExerciseIdAsync(List<int> planExerciseIds);
    }
}
