using PowerLifting.Domain.DbModels.UserData;
using PowerLifting.Infrastructure.Setup;
using PowerLifting.Infrastructure.Setup.Generic.Repository;

namespace PowerLifting.Infrastructure.Repositories.UserData
{
    public class UserInfoRepository : CrudRepo<UserInfoDb>
    {
        public UserInfoRepository(IContextProvider provider) : base(provider) { }
    }
}
