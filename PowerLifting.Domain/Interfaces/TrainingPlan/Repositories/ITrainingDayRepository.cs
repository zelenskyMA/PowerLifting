using PowerLifting.Domain.DbModels.TrainingPlan;
using PowerLifting.Domain.Interfaces.Common.Repositories;

namespace PowerLifting.Domain.Interfaces.TrainingPlan.Repositories
{
    public interface ITrainingDayRepository : ICrudRepo<TrainingDayDb>
    {
    }
}
