using SportAssistant.Domain.DbModels.Management;
using SportAssistant.Infrastructure.Common;
using SportAssistant.Infrastructure.DataContext;

namespace SportAssistant.Infrastructure.Repositories.Management;

public class ManagerRepository : CrudRepo<ManagerDb>
{
    public ManagerRepository(IContextProvider provider) : base(provider)
    {
    }
}
