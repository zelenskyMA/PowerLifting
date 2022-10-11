using PowerLifting.Domain.Models.TrainingPlan;

namespace PowerLifting.Domain.Interfaces.TrainingPlan.Application
{
    public interface IExerciseCommands
    {
        /// <summary>
        /// Get exercise by id. Closed allowed for back compatibility.
        /// For plans only.
        /// </summary>
        /// <param name="id">Exercise Id</param>
        /// <returns></returns>
        Task<Exercise?> GetAsync(int id);

        /// <summary>
        /// Get exercises by id list. Closed allowed for back compatibility.
        /// For plans only.
        /// </summary>
        /// <param name="ids">Exercise Ids</param>
        /// <returns></returns>
        Task<List<Exercise>> GetAsync(List<int> ids);

        /// <summary>
        /// Get exercise list for day train planing
        /// </summary>
        /// <returns></returns>
        Task<List<Exercise>> GetPlanningListAsync();

        /// <summary>
        /// Get custom exercise list for user editing
        /// </summary>
        /// <returns></returns>
        Task<List<Exercise>> GetEditingListAsync();

        /// <summary>
        /// Get basic exercise list for admin editing
        /// </summary>
        /// <returns></returns>
        Task<List<Exercise>> GetAdminEditingListAsync();

        /// <summary>
        /// Create / Update exercise. Basic as well for Admin only.
        /// </summary>
        /// <param name="exercise">Entity to create / update</param>
        /// <returns></returns>
        Task UpdateAsync(Exercise exercise);

        /// <summary>
        /// Delete exercise. Basic as well for Admin only.
        /// </summary>
        /// <param name="id">exercise Id</param>
        /// <returns></returns>
        Task DeleteAsync(int id);
    }
}
