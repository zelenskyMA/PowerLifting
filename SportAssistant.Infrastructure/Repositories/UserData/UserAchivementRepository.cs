using SportAssistant.Domain.DbModels.UserData;
using SportAssistant.Infrastructure.Common;
using SportAssistant.Infrastructure.DataContext;

namespace SportAssistant.Infrastructure.Repositories.UserData;

public class UserAchivementRepository : CrudRepo<UserAchivementDb>
{
    public UserAchivementRepository(IContextProvider provider) : base(provider) { }
}
