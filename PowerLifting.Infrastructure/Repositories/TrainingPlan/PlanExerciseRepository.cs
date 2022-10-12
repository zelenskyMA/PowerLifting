using PowerLifting.Domain.DbModels.TrainingPlan;
using PowerLifting.Infrastructure.Repositories.Common;
using PowerLifting.Infrastructure.Setup;

namespace PowerLifting.Infrastructure.Repositories.TrainingPlan
{
    public class PlanExerciseRepository : CrudRepo<PlanExerciseDb>
    {
        public PlanExerciseRepository(IContextProvider provider) : base(provider)
        {
        }
    }
}
