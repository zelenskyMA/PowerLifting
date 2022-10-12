using PowerLifting.Domain.DbModels.TrainingPlan;
using PowerLifting.Infrastructure.Setup;
using PowerLifting.Infrastructure.Setup.Generic.Repository;

namespace PowerLifting.Infrastructure.Repositories.TrainingPlan
{
    public class PlanRepository : CrudRepo<PlanDb>
    {
        public PlanRepository(IContextProvider provider) : base(provider) { }
    }
}
