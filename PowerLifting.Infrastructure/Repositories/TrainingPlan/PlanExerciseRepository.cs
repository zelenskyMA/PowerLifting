using PowerLifting.Domain.DbModels.TrainingPlan;
using PowerLifting.Infrastructure.Common;
using PowerLifting.Infrastructure.DataContext;

namespace PowerLifting.Infrastructure.Repositories.TrainingPlan
{
    public class PlanExerciseRepository : CrudRepo<PlanExerciseDb>
    {
        public PlanExerciseRepository(IContextProvider provider) : base(provider)
        {
        }
    }
}
