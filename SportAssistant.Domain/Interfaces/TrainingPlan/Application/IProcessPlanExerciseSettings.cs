using SportAssistant.Domain.Models.TrainingPlan;

namespace SportAssistant.Domain.Interfaces.TrainingPlan.Application
{
    public interface IProcessPlanExerciseSettings
    {
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
