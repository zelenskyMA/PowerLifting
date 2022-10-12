using PowerLifting.Domain.DbModels.UserData;
using PowerLifting.Infrastructure.Setup;
using PowerLifting.Infrastructure.Setup.Generic.Repository;

namespace PowerLifting.Infrastructure.Repositories.UserData
{
    public class UserRoleRepository : CrudRepo<UserRoleDb>
    {
        public UserRoleRepository(IContextProvider provider) : base(provider) { }
    }
}
