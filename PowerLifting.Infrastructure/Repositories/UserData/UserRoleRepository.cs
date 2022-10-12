using PowerLifting.Domain.DbModels.UserData;
using PowerLifting.Infrastructure.Repositories.Common;
using PowerLifting.Infrastructure.Setup;

namespace PowerLifting.Infrastructure.Repositories.UserData
{
    public class UserRoleRepository : CrudRepo<UserRoleDb>
    {
        public UserRoleRepository(IContextProvider provider) : base(provider) { }
    }
}
