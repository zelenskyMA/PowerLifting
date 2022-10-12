using Microsoft.EntityFrameworkCore;
using PowerLifting.Domain.DbModels.TrainingPlan;
using PowerLifting.Domain.Interfaces.TrainingPlan.Repositories;
using PowerLifting.Infrastructure.Repositories.Common;
using PowerLifting.Infrastructure.Setup;

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
