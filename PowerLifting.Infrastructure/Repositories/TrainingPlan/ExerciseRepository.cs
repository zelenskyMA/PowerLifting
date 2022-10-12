using PowerLifting.Domain.DbModels.TrainingPlan;
using PowerLifting.Infrastructure.Repositories.Common;
using PowerLifting.Infrastructure.Setup;

namespace PowerLifting.Infrastructure.Repositories.TrainingPlan
{
    public class ExerciseRepository : CrudRepo<ExerciseDb>
    {
        public ExerciseRepository(IContextProvider provider) : base(provider)
        {
        }
    }
}
