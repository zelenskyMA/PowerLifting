using PowerLifting.Domain.Models.TrainingPlan;

namespace PowerLifting.Domain.Interfaces.TrainingPlan.Application
{
    public interface IExerciseCommands
    {
        /// <summary>
        /// Get full exercise list
        /// </summary>
        /// <returns></returns>
        Task<List<Exercise>> GetListAsync();

        /// <summary>
        /// Get exercises by id list
        /// </summary>
        /// <param name="ids">Exercise Ids</param>
        /// <returns></returns>
        Task<List<Exercise>> GetAsync(List<int> ids);
    }
}
