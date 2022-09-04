using PowerLifting.Domain.Models.TrainingPlan;

namespace PowerLifting.Domain.Interfaces.TrainingPlan.Application
{
  public interface IExerciseApp
  {
    Task<List<Exercise>> GetListAsync();
  }
}
