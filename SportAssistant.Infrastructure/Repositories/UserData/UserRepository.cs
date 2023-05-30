using SportAssistant.Domain.DbModels.UserData;
using SportAssistant.Infrastructure.Common;
using SportAssistant.Infrastructure.DataContext;

namespace SportAssistant.Infrastructure.Repositories.UserData;

public class UserRepository : CrudRepo<UserDb>
{
    public UserRepository(IContextProvider provider) : base(provider) { }
}
