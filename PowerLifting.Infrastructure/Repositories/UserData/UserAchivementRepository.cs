using PowerLifting.Domain.DbModels.UserData;
using PowerLifting.Infrastructure.Setup;
using PowerLifting.Infrastructure.Setup.Generic.Repository;

namespace PowerLifting.Infrastructure.Repositories.UserData
{
    public class UserAchivementRepository : CrudRepo<UserAchivementDb>
    {
        public UserAchivementRepository(IContextProvider provider) : base(provider) { }
    }
}
