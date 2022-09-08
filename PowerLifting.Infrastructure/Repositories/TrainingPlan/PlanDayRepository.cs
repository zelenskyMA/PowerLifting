using Microsoft.EntityFrameworkCore;
using PowerLifting.Domain.DbModels.TrainingPlan;
using PowerLifting.Infrastructure.Repositories.Common;

namespace PowerLifting.Infrastructure.Repositories.TrainingPlan
{
    public class PlanDayRepository : CrudRepo<PlanDayDb>
    {
        public PlanDayRepository(DbContextOptions<LiftingContext> provider) : base(provider) { }
    }
}
