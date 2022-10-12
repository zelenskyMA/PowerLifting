using PowerLifting.Domain.DbModels.UserData;
using PowerLifting.Infrastructure.Repositories.Common;
using PowerLifting.Infrastructure.Setup;

namespace PowerLifting.Infrastructure.Repositories.UserData
{
    public class UserRepository : CrudRepo<UserDb>
    {
        public UserRepository(IContextProvider provider) : base(provider) { }
    }
}
