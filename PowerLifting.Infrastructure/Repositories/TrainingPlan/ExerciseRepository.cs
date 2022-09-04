using Microsoft.EntityFrameworkCore;
using PowerLifting.Domain.DbModels.TrainingPlan;
using PowerLifting.Domain.Interfaces.TrainingPlan.Repositories;
using PowerLifting.Infrastructure.Repositories.Common;

namespace PowerLifting.Infrastructure.Repositories.TrainingPlan
{
    public class ExerciseRepository : CrudRepo<ExerciseDb>, IExerciseRepository
    {
        protected DbSet<ExerciseTypeDb> ExerciseTypeDbSet { get; set; }

        public ExerciseRepository(DbContextOptions<LiftingContext> provider) : base(provider)
        {
            ExerciseTypeDbSet = Context.Set<ExerciseTypeDb>();
        }

        public async Task<IList<ExerciseTypeDb>> GetExerciseTypesAsync() => await ExerciseTypeDbSet.ToListAsync();

    }
}
