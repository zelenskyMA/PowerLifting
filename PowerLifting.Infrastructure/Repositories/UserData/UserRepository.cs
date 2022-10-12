using PowerLifting.Domain.DbModels.UserData;
using PowerLifting.Infrastructure.Setup;
using PowerLifting.Infrastructure.Setup.Generic.Repository;

namespace PowerLifting.Infrastructure.Repositories.UserData
{
    public class UserRepository : CrudRepo<UserDb>
    {
        public UserRepository(IContextProvider provider) : base(provider) { }
    }
}
