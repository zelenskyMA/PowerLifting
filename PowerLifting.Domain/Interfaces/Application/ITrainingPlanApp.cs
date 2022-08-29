using PowerLifting.Domain.Models.TrainingWork;

namespace PowerLifting.Domain.Interfaces.Application
{
  public interface ITrainingPlanApp
  {
    Task<TrainingPlan> GetAsync(int Id);

    Task<int> UpdateAsync(TrainingPlan plan);
  }
}
