using Microsoft.EntityFrameworkCore;
using PowerLifting.Domain.DbModels.TrainingPlan;
using PowerLifting.Infrastructure.Repositories.Common;

namespace PowerLifting.Infrastructure.Repositories.TrainingPlan
{
    public class ExercisePercentageRepository : CrudRepo<ExercisePercentageDb>
    {
        public ExercisePercentageRepository(DbContextOptions<LiftingContext> provider) : base(provider)
        {
        }
    }
}
