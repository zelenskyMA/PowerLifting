using PowerLifting.Domain.DbModels.UserData;
using PowerLifting.Infrastructure.Repositories.Common;
using PowerLifting.Infrastructure.Setup;

namespace PowerLifting.Infrastructure.Repositories.UserData
{
    public class UserInfoRepository : CrudRepo<UserInfoDb>
    {
        public UserInfoRepository(IContextProvider provider) : base(provider) { }
    }
}
