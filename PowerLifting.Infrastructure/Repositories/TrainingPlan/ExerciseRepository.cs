using PowerLifting.Domain.DbModels.TrainingPlan;
using PowerLifting.Infrastructure.Common;
using PowerLifting.Infrastructure.DataContext;

namespace PowerLifting.Infrastructure.Repositories.TrainingPlan
{
    public class ExerciseRepository : CrudRepo<ExerciseDb>
    {
        public ExerciseRepository(IContextProvider provider) : base(provider)
        {
        }
    }
}
