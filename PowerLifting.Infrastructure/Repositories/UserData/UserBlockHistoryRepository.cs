using PowerLifting.Domain.DbModels.UserData;
using PowerLifting.Infrastructure.Common;
using PowerLifting.Infrastructure.DataContext;

namespace PowerLifting.Infrastructure.Repositories.UserData
{
    public class UserBlockHistoryRepository : CrudRepo<UserBlockHistoryDb>
    {
        public UserBlockHistoryRepository(IContextProvider provider) : base(provider) { }
    }
}
