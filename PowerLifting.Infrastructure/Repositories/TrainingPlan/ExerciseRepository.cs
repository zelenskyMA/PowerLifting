using Microsoft.EntityFrameworkCore;
using PowerLifting.Domain.DbModels.TrainingPlan;
using PowerLifting.Domain.Interfaces.TrainingPlan.Repositories;
using PowerLifting.Infrastructure.Repositories.Common;

namespace PowerLifting.Infrastructure.Repositories.TrainingPlan
{
    public class ExerciseRepository : CrudRepo<ExerciseDb>, IExerciseRepository
    {
        public ExerciseRepository(DbContextOptions<LiftingContext> provider) : base(provider)
        {
        }
    }
}
