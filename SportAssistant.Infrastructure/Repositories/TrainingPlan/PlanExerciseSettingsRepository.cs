using Microsoft.EntityFrameworkCore;
using SportAssistant.Domain.DbModels.TrainingPlan;
using SportAssistant.Domain.Interfaces.TrainingPlan.Repositories;
using SportAssistant.Infrastructure.Common;
using SportAssistant.Infrastructure.DataContext;

namespace SportAssistant.Infrastructure.Repositories.TrainingPlan
{
    public class PlanExerciseSettingsRepository : CrudRepo<PlanExerciseSettingsDb>, IPlanExerciseSettingsRepository
    {
        public PlanExerciseSettingsRepository(IContextProvider provider) : base(provider)
        {
        }

        public async Task<List<PercentageDb>> GetPercentagesAsync() => await Context.Percentages.ToListAsync();
    }
}
