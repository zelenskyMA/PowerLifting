using PowerLifting.Domain.Models.TrainingPlan;

namespace PowerLifting.Domain.Interfaces.TrainingPlan.Application
{
    public interface IPlanExerciseCommands
    {
        /// <summary>
        /// Get planned exercises by training day Id
        /// </summary>
        /// <param name="dayId">Training day Id</param>
        /// <returns>List of planned exercises</returns>
        Task<List<PlanExercise>> GetAsync(int dayId);

        /// <summary>
        /// Get planned exercises by list of training day Ids
        /// </summary>
        /// <param name="dayId">List of training day Ids</param>
        /// <returns>List of planned exercises</returns>
        Task<List<PlanExercise>> GetAsync(List<int> dayIds);

        /// <summary>
        /// Create new planned exercises set for training plan
        /// </summary>
        /// <param name="trainingDayId">Id of the day in training plan</param>
        /// <param name="exercises">Selected exercises for planning</param>
        /// <returns></returns>
        Task CreateAsync(int dayId, List<Exercise> exercises);
    }
}
