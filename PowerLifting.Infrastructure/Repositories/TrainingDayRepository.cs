using Microsoft.EntityFrameworkCore;
using PowerLifting.Domain.DbModels;
using PowerLifting.Domain.Interfaces.Repositories;
using PowerLifting.Infrastructure.Repositories.Common;

namespace PowerLifting.Infrastructure.Repositories
{
  public class TrainingDayRepository : CrudRepo<TrainingDayDb>, ITrainingDayRepository
  {
    public TrainingDayRepository(DbContextOptions<LiftingContext> provider) : base(provider) { }
  }
}
