using PowerLifting.Domain.DbModels;
using PowerLifting.Domain.Interfaces.Repositories.Common;

namespace PowerLifting.Domain.Interfaces.Repositories
{
  public interface IExerciseRepository : ICrudRepo<ExerciseDb>
  {
    Task<IList<ExerciseTypeDb>> GetExerciseTypesAsync();
  }
}
