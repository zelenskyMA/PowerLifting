using Microsoft.EntityFrameworkCore;
using PowerLifting.Domain.DbModels.Coaching;
using PowerLifting.Infrastructure.Repositories.Common;

namespace PowerLifting.Infrastructure.Repositories.Coaching
{
    public class TrainingGroupRepository : CrudRepo<TrainingGroupDb>
    {
        public TrainingGroupRepository(DbContextOptions<LiftingContext> provider) : base(provider) { }

    }
}
