using SportAssistant.Domain.DbModels.UserData;
using SportAssistant.Infrastructure.Common;
using SportAssistant.Infrastructure.DataContext;

namespace SportAssistant.Infrastructure.Repositories.UserData;

public class UserRoleRepository : CrudRepo<UserRoleDb>
{
    public UserRoleRepository(IContextProvider provider) : base(provider) { }
}
