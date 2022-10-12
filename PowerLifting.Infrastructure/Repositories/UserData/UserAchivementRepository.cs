using PowerLifting.Domain.DbModels.UserData;
using PowerLifting.Infrastructure.Repositories.Common;
using PowerLifting.Infrastructure.Setup;

namespace PowerLifting.Infrastructure.Repositories.UserData
{
    public class UserAchivementRepository : CrudRepo<UserAchivementDb>
    {
        public UserAchivementRepository(IContextProvider provider) : base(provider) { }
    }
}
