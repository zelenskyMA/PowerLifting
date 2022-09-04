using Microsoft.EntityFrameworkCore;
using PowerLifting.Domain.DbModels.TrainingPlan;
using PowerLifting.Domain.Interfaces.TrainingPlan.Repositories;
using PowerLifting.Infrastructure.Repositories.Common;

namespace PowerLifting.Infrastructure.Repositories.TrainingPlan
{
    public class PlannedExerciseRepository : CrudRepo<PlannedExerciseDb>, IPlannedExerciseRepository
    {
        public PlannedExerciseRepository(DbContextOptions<LiftingContext> provider) : base(provider)
        {
        }
    }
}
