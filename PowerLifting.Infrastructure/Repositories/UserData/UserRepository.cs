using PowerLifting.Domain.DbModels.UserData;
using PowerLifting.Infrastructure.Common;
using PowerLifting.Infrastructure.DataContext;

namespace PowerLifting.Infrastructure.Repositories.UserData
{
    public class UserRepository : CrudRepo<UserDb>
    {
        public UserRepository(IContextProvider provider) : base(provider) { }
    }
}
