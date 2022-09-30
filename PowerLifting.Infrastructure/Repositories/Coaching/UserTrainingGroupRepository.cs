using Microsoft.EntityFrameworkCore;
using PowerLifting.Domain.DbModels.Coaching;
using PowerLifting.Infrastructure.Repositories.Common;

namespace PowerLifting.Infrastructure.Repositories.Coaching
{
    public class UserTrainingGroupRepository : CrudRepo<UserTrainingGroupDb>
    {
        public UserTrainingGroupRepository(DbContextOptions<LiftingContext> provider) : base(provider) { }

    }
}
