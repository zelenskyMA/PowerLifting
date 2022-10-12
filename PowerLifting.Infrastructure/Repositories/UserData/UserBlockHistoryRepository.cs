using PowerLifting.Domain.DbModels.UserData;
using PowerLifting.Infrastructure.Repositories.Common;
using PowerLifting.Infrastructure.Setup;

namespace PowerLifting.Infrastructure.Repositories.UserData
{
    public class UserBlockHistoryRepository : CrudRepo<UserBlockHistoryDb>
    {
        public UserBlockHistoryRepository(IContextProvider provider) : base(provider) { }
    }
}
