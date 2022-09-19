using Microsoft.EntityFrameworkCore;
using PowerLifting.Domain.DbModels.UserData;
using PowerLifting.Infrastructure.Repositories.Common;

namespace PowerLifting.Infrastructure.Repositories.UserData
{
    public class UserRoleRepository : CrudRepo<UserRoleDb>
    {
        public UserRoleRepository(DbContextOptions<LiftingContext> provider) : base(provider) { }
    }
}
