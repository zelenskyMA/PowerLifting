using Microsoft.EntityFrameworkCore;
using PowerLifting.Domain.DbModels.UserData;
using PowerLifting.Infrastructure.Repositories.Common;

namespace PowerLifting.Infrastructure.Repositories.UserData
{
    public class UserAchivementRepository : CrudRepo<UserAchivementDb>
    {
        public UserAchivementRepository(DbContextOptions<LiftingContext> provider) : base(provider) { }
    }
}
