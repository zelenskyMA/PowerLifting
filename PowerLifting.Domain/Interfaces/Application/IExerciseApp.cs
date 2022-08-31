using PowerLifting.Domain.Models.TrainingWork;

namespace PowerLifting.Domain.Interfaces.Application
{
  public interface IExerciseApp
  {
    Task<List<Exercise>> GetListAsync();
  }
}
