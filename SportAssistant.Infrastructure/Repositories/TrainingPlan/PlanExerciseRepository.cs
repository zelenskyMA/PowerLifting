using SportAssistant.Domain.DbModels.TrainingPlan;
using SportAssistant.Infrastructure.Common;
using SportAssistant.Infrastructure.DataContext;

namespace SportAssistant.Infrastructure.Repositories.TrainingPlan;

public class PlanExerciseRepository : CrudRepo<PlanExerciseDb>
{
    public PlanExerciseRepository(IContextProvider provider) : base(provider)
    {
    }
}
