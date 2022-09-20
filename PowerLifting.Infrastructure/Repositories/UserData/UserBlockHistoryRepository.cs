using Microsoft.EntityFrameworkCore;
using PowerLifting.Domain.DbModels.UserData;
using PowerLifting.Infrastructure.Repositories.Common;

namespace PowerLifting.Infrastructure.Repositories.UserData
{
    public class UserBlockHistoryRepository : CrudRepo<UserBlockHistoryDb>
    {
        public UserBlockHistoryRepository(DbContextOptions<LiftingContext> provider) : base(provider) { }
    }
}
