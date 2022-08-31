using Microsoft.EntityFrameworkCore;
using PowerLifting.Domain.DbModels;
using PowerLifting.Domain.Interfaces.Repositories;
using PowerLifting.Infrastructure.Repositories.Common;

namespace PowerLifting.Infrastructure.Repositories
{
  public class PlannedExerciseRepository : CrudRepo<PlannedExerciseDb>, IPlannedExerciseRepository
  {
    public PlannedExerciseRepository(DbContextOptions<LiftingContext> provider) : base(provider)
    {
    }
  }
}
