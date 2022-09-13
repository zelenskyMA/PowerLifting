using Microsoft.EntityFrameworkCore;
using PowerLifting.Domain.DbModels.UserData;
using PowerLifting.Infrastructure.Repositories.Common;

namespace PowerLifting.Infrastructure.Repositories.UserData
{
    public class UserInfoRepository : CrudRepo<UserInfoDb>
    {
        public UserInfoRepository(DbContextOptions<LiftingContext> provider) : base(provider) { }
    }
}
