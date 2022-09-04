using Microsoft.EntityFrameworkCore;
using PowerLifting.Domain.DbModels.TrainingPlan;
using PowerLifting.Domain.Interfaces.TrainingPlan.Repositories;
using PowerLifting.Infrastructure.Repositories.Common;

namespace PowerLifting.Infrastructure.Repositories.TrainingPlan
{
    public class ExerciseSettingsRepository : CrudRepo<ExerciseSettingsDb>, IExerciseSettingsRepository
    {
        public ExerciseSettingsRepository(DbContextOptions<LiftingContext> provider) : base(provider)
        {
        }
    }
}
