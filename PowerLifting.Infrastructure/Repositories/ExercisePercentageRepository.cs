using Microsoft.EntityFrameworkCore;
using PowerLifting.Domain.DbModels;
using PowerLifting.Domain.Interfaces.Repositories;
using PowerLifting.Infrastructure.Repositories.Common;

namespace PowerLifting.Infrastructure.Repositories
{
  public class ExercisePercentageRepository : CrudRepo<ExercisePercentageDb>, IExercisePercentageRepository
  {
    public ExercisePercentageRepository(DbContextOptions<LiftingContext> provider) : base(provider)
    {
    }
  }
}
