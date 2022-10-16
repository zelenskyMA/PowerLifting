using PowerLifting.Domain.DbModels.TrainingPlan;
using PowerLifting.Infrastructure.Common;
using PowerLifting.Infrastructure.DataContext;

namespace PowerLifting.Infrastructure.Repositories.TrainingPlan
{
    public class PlanDayRepository : CrudRepo<PlanDayDb>
    {
        public PlanDayRepository(IContextProvider provider) : base(provider) { }
    }
}
