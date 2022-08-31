using PowerLifting.Domain.DbModels;

namespace PowerLifting.Domain.Interfaces.Repositories
{

  public interface IExerciseRepository : ICrudRepo<ExerciseDb>
  {
    Task<IList<ExerciseTypeDb>> GetExerciseTypesAsync();
  }
}
