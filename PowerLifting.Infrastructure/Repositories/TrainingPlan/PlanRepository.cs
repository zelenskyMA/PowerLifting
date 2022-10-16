using PowerLifting.Domain.DbModels.TrainingPlan;
using PowerLifting.Infrastructure.Common;
using PowerLifting.Infrastructure.DataContext;

namespace PowerLifting.Infrastructure.Repositories.TrainingPlan
{
    public class PlanRepository : CrudRepo<PlanDb>
    {
        public PlanRepository(IContextProvider provider) : base(provider) { }
    }
}
