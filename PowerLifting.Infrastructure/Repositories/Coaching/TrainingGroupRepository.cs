using Microsoft.EntityFrameworkCore;
using PowerLifting.Domain.DbModels.Coaching;
using PowerLifting.Infrastructure.Repositories.Common;
using PowerLifting.Infrastructure.Setup;

namespace PowerLifting.Infrastructure.Repositories.Coaching
{
    public class TrainingGroupRepository : CrudRepo<TrainingGroupDb>
    {
        public TrainingGroupRepository(IContextProvider provider) : base(provider) { }
    }
}
