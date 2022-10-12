using PowerLifting.Domain.DbModels.TrainingPlan;
using PowerLifting.Infrastructure.Repositories.Common;
using PowerLifting.Infrastructure.Setup;

namespace PowerLifting.Infrastructure.Repositories.TrainingPlan
{
    public class PlanRepository : CrudRepo<PlanDb>
    {
        public PlanRepository(IContextProvider provider) : base(provider) { }
    }
}
