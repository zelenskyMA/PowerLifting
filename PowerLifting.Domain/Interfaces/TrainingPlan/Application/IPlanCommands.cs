using PowerLifting.Domain.Models.TrainingPlan;

namespace PowerLifting.Domain.Interfaces.TrainingPlan.Application
{
    public interface IPlanCommands
    {
        /// <summary>
        /// Get user training plans
        /// </summary>
        /// <returns></returns>
        Task<Plans> GetPlansAsync();

        /// <summary>
        /// Get training plan
        /// </summary>
        /// <param name="Id">Plan Id</param>
        /// <returns>Training plan</returns>
        Task<Plan> GetPlanAsync(int Id);

        /// <summary>
        /// Get training plan day
        /// </summary>
        /// <param name="dayId">training day Id</param>
        /// <returns></returns>
        Task<PlanDay> GetPlanDayAsync(int dayId);

        /// <summary>
        /// Create new training plan with training days
        /// </summary>
        /// <param name="creationDate">Plan start date</param>
        /// <returns>Plan Id</returns>
        Task<int> CreateAsync(DateTime creationDate);
    }
}
