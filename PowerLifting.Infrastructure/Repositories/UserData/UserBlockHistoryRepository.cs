using PowerLifting.Domain.DbModels.UserData;
using PowerLifting.Infrastructure.Setup;
using PowerLifting.Infrastructure.Setup.Generic.Repository;

namespace PowerLifting.Infrastructure.Repositories.UserData
{
    public class UserBlockHistoryRepository : CrudRepo<UserBlockHistoryDb>
    {
        public UserBlockHistoryRepository(IContextProvider provider) : base(provider) { }
    }
}
