using SportAssistant.Domain.DbModels.TrainingPlan;
using SportAssistant.Infrastructure.Common;
using SportAssistant.Infrastructure.DataContext;

namespace SportAssistant.Infrastructure.Repositories.TrainingPlan;

public class PlanDayRepository : CrudRepo<PlanDayDb>
{
    public PlanDayRepository(IContextProvider provider) : base(provider) { }
}
