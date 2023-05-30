using SportAssistant.Domain.DbModels.UserData;
using SportAssistant.Infrastructure.Common;
using SportAssistant.Infrastructure.DataContext;

namespace SportAssistant.Infrastructure.Repositories.UserData;

public class UserInfoRepository : CrudRepo<UserInfoDb>
{
    public UserInfoRepository(IContextProvider provider) : base(provider) { }
}
