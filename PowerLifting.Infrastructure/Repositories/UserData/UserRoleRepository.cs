using PowerLifting.Domain.DbModels.UserData;
using PowerLifting.Infrastructure.Common;
using PowerLifting.Infrastructure.DataContext;

namespace PowerLifting.Infrastructure.Repositories.UserData
{
    public class UserRoleRepository : CrudRepo<UserRoleDb>
    {
        public UserRoleRepository(IContextProvider provider) : base(provider) { }
    }
}
