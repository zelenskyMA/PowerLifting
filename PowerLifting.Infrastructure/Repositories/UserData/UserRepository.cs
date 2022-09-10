using Microsoft.EntityFrameworkCore;
using PowerLifting.Domain.DbModels.UserData;
using PowerLifting.Infrastructure.Repositories.Common;

namespace PowerLifting.Infrastructure.Repositories.UserData
{
    public class UserRepository : CrudRepo<UserDb>
    {
        public UserRepository(DbContextOptions<LiftingContext> provider) : base(provider) { }
    }
}
