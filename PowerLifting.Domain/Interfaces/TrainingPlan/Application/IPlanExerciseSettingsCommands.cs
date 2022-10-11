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
        /// Update exercise settings for plan exercise
        /// </summary>
        /// <param name="planExerciseId">Plan exercise Id</param>
        /// <param name="achivement">User achivement in target exercise type</param>
        /// <param name="settingsList">List of plan exercise settings to update</param>
        /// <returns></returns>
        Task UpdateAsync(int planExerciseId, int achivement, List<PlanExerciseSettings> settingsList);

        /// <summary>
        /// Complete exercises in selected percentage span
        /// </summary>
        /// <param name="exerciseIds">List of exercise Ids to complete</param>
        /// <returns></returns>
        Task CompleteExercisesAsync(List<int> exerciseIds);

        /// <summary>
        /// Delete all supplied items
        /// </summary>
        /// <param name="settings">Settings for deleting</param>
        /// <returns></returns>
        Task DeleteByPlanExerciseIdAsync(List<int> planExerciseIds);

        /// <summary>
        /// Get full list of current planDay percentages
        /// </summary>
        /// <returns></returns>
        Task<List<Percentage>> GetPercentageListAsync();
    }
}
