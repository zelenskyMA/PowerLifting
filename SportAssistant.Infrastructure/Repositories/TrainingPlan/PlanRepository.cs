using SportAssistant.Domain.DbModels.TrainingPlan;
using SportAssistant.Infrastructure.Common;
using SportAssistant.Infrastructure.DataContext;

namespace SportAssistant.Infrastructure.Repositories.TrainingPlan;

public class PlanRepository : CrudRepo<PlanDb>
{
    public PlanRepository(IContextProvider provider) : base(provider) { }
}
