using Microsoft.EntityFrameworkCore;
using PowerLifting.Domain.DbModels;
using PowerLifting.Domain.Interfaces.Repositories;
using PowerLifting.Infrastructure.Repositories.Common;

namespace PowerLifting.Infrastructure.Repositories
{
  public class PercentageRepository : CrudRepo<PercentageDb>, IPercentageRepository
  {
    public PercentageRepository(DbContextOptions<LiftingContext> provider) : base(provider)
    {
    }
  }
}
