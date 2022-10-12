using PowerLifting.Domain.DbModels.TrainingPlan;
using PowerLifting.Infrastructure.Repositories.Common;
using PowerLifting.Infrastructure.Setup;

namespace PowerLifting.Infrastructure.Repositories.TrainingPlan
{
    public class PlanDayRepository : CrudRepo<PlanDayDb>
    {
        public PlanDayRepository(IContextProvider provider) : base(provider) { }
    }
}
