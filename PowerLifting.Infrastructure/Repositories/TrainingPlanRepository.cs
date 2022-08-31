using Microsoft.EntityFrameworkCore;
using PowerLifting.Domain.DbModels;
using PowerLifting.Domain.Interfaces.Repositories;
using PowerLifting.Infrastructure.Repositories.Common;

namespace PowerLifting.Infrastructure.Repositories
{
  public class TrainingPlanRepository : CrudRepo<TrainingPlanDb>, ITrainingPlanRepository
  {
    public TrainingPlanRepository(DbContextOptions<LiftingContext> provider) : base(provider) { }
  }
}
