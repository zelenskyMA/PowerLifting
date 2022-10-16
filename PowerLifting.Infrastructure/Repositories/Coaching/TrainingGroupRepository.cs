using PowerLifting.Domain.DbModels.Coaching;
using PowerLifting.Infrastructure.Common;
using PowerLifting.Infrastructure.DataContext;

namespace PowerLifting.Infrastructure.Repositories.Coaching
{
    public class TrainingGroupRepository : CrudRepo<TrainingGroupDb>
    {
        public TrainingGroupRepository(IContextProvider provider) : base(provider) { }
    }
}
