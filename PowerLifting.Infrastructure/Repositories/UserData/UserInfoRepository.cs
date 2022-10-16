using PowerLifting.Domain.DbModels.UserData;
using PowerLifting.Infrastructure.Common;
using PowerLifting.Infrastructure.DataContext;

namespace PowerLifting.Infrastructure.Repositories.UserData
{
    public class UserInfoRepository : CrudRepo<UserInfoDb>
    {
        public UserInfoRepository(IContextProvider provider) : base(provider) { }
    }
}
