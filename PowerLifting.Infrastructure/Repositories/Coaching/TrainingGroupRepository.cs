using PowerLifting.Domain.DbModels.Coaching;
using PowerLifting.Infrastructure.Setup;
using PowerLifting.Infrastructure.Setup.Generic.Repository;

namespace PowerLifting.Infrastructure.Repositories.Coaching
{
    public class TrainingGroupRepository : CrudRepo<TrainingGroupDb>
    {
        public TrainingGroupRepository(IContextProvider provider) : base(provider) { }
    }
}
