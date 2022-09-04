using PowerLifting.Domain.Models.TrainingPlan;

namespace PowerLifting.Domain.Interfaces.TrainingPlan.Application
{
  public interface ITrainingPlanApp
  {
    /// <summary>
    /// Get training plan
    /// </summary>
    /// <param name="Id">Plan Id</param>
    /// <returns>Training plan</returns>
    Task<TrainingPlanModel> GetAsync(int Id);

    /// <summary>
    /// Create new training plan with training days
    /// </summary>
    /// <param name="creationDate">Plan start date</param>
    /// <returns>Plan Id</returns>
    Task<int> CreateAsync(DateTime creationDate);
  }
}
