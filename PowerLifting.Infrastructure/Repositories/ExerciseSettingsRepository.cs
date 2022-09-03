using Microsoft.EntityFrameworkCore;
using PowerLifting.Domain.DbModels;
using PowerLifting.Domain.Interfaces.Repositories;
using PowerLifting.Infrastructure.Repositories.Common;

namespace PowerLifting.Infrastructure.Repositories
{
  public class ExerciseSettingsRepository : CrudRepo<ExerciseSettingsDb>, IExerciseSettingsRepository
  {
    public ExerciseSettingsRepository(DbContextOptions<LiftingContext> provider) : base(provider)
    {
    }
  }
}
