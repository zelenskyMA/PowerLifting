using Microsoft.EntityFrameworkCore;
using PowerLifting.Domain.DbModels.TrainingPlan;
using PowerLifting.Infrastructure.Repositories.Common;

namespace PowerLifting.Infrastructure.Repositories.TrainingPlan
{
    public class PlanExerciseRepository : CrudRepo<PlanExerciseDb>
    {
        public PlanExerciseRepository(DbContextOptions<LiftingContext> provider) : base(provider)
        {
        }
    }
}
