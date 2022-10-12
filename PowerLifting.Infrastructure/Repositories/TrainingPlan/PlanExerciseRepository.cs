using PowerLifting.Domain.DbModels.TrainingPlan;
using PowerLifting.Infrastructure.Setup;
using PowerLifting.Infrastructure.Setup.Generic.Repository;

namespace PowerLifting.Infrastructure.Repositories.TrainingPlan
{
    public class PlanExerciseRepository : CrudRepo<PlanExerciseDb>
    {
        public PlanExerciseRepository(IContextProvider provider) : base(provider)
        {
        }
    }
}
