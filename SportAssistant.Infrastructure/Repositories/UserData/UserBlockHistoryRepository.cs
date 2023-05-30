using SportAssistant.Domain.DbModels.UserData;
using SportAssistant.Infrastructure.Common;
using SportAssistant.Infrastructure.DataContext;

namespace SportAssistant.Infrastructure.Repositories.UserData;

public class UserBlockHistoryRepository : CrudRepo<UserBlockHistoryDb>
{
    public UserBlockHistoryRepository(IContextProvider provider) : base(provider) { }
}
