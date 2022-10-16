using Microsoft.EntityFrameworkCore;
using PowerLifting.Domain.DbModels.TrainingPlan;
using PowerLifting.Domain.Interfaces.TrainingPlan.Repositories;
using PowerLifting.Infrastructure.Common;
using PowerLifting.Infrastructure.DataContext;

namespace PowerLifting.Infrastructure.Repositories.TrainingPlan
{
    public class PlanExerciseSettingsRepository : CrudRepo<PlanExerciseSettingsDb>, IPlanExerciseSettingsRepository
    {
        public PlanExerciseSettingsRepository(IContextProvider provider) : base(provider)
        {
        }

        public async Task<List<PercentageDb>> GetPercentagesAsync() => await Context.Percentages.ToListAsync();
    }
}
