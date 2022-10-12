using PowerLifting.Domain.DbModels.TrainingPlan;
using PowerLifting.Infrastructure.Setup;
using PowerLifting.Infrastructure.Setup.Generic.Repository;

namespace PowerLifting.Infrastructure.Repositories.TrainingPlan
{
    public class PlanDayRepository : CrudRepo<PlanDayDb>
    {
        public PlanDayRepository(IContextProvider provider) : base(provider) { }
    }
}
