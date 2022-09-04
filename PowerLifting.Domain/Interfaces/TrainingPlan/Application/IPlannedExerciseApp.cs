using PowerLifting.Domain.Models.TrainingPlan;

namespace PowerLifting.Domain.Interfaces.TrainingPlan.Application
{
  public interface IPlannedExerciseApp
  {
    /// <summary>
    /// Create new planned exercises set for training plan
    /// </summary>
    /// <param name="trainingDayId">Id of the day in training plan</param>
    /// <param name="exercises">Selected exercises for planning</param>
    /// <returns></returns>
    Task CreateAsync(int planDayId, List<Exercise> exercises);
  }
}
