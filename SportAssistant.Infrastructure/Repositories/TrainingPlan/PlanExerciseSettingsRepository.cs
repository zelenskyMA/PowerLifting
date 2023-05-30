using SportAssistant.Domain.DbModels.TrainingPlan;
using SportAssistant.Infrastructure.Common;
using SportAssistant.Infrastructure.DataContext;

namespace SportAssistant.Infrastructure.Repositories.TrainingPlan;

public class PlanExerciseSettingsRepository : CrudRepo<PlanExerciseSettingsDb>
{
    public PlanExerciseSettingsRepository(IContextProvider provider) : base(provider)
    {
    }
}
