using Microsoft.EntityFrameworkCore;
using PowerLifting.Domain.DbModels.TrainingPlan;
using PowerLifting.Infrastructure.Repositories.Common;

namespace PowerLifting.Infrastructure.Repositories.TrainingPlan
{
    public class PlanRepository : CrudRepo<PlanDb>
    {
        public PlanRepository(DbContextOptions<LiftingContext> provider) : base(provider) { }
    }
}
