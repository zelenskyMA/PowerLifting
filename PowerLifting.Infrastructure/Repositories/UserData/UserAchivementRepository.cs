using PowerLifting.Domain.DbModels.UserData;
using PowerLifting.Infrastructure.Common;
using PowerLifting.Infrastructure.DataContext;

namespace PowerLifting.Infrastructure.Repositories.UserData
{
    public class UserAchivementRepository : CrudRepo<UserAchivementDb>
    {
        public UserAchivementRepository(IContextProvider provider) : base(provider) { }
    }
}
