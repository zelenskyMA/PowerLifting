using PowerLifting.Domain.DbModels.TrainingPlan;
using PowerLifting.Infrastructure.Setup;
using PowerLifting.Infrastructure.Setup.Generic.Repository;

namespace PowerLifting.Infrastructure.Repositories.TrainingPlan
{
    public class ExerciseRepository : CrudRepo<ExerciseDb>
    {
        public ExerciseRepository(IContextProvider provider) : base(provider)
        {
        }
    }
}
