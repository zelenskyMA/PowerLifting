using SportAssistant.Domain.DbModels.Management;
using SportAssistant.Infrastructure.Common;
using SportAssistant.Infrastructure.DataContext;

namespace SportAssistant.Infrastructure.Repositories.Management;

public class AssignedCoachRepository : CrudRepo<AssignedCoachDb>
{
    public AssignedCoachRepository(IContextProvider provider) : base(provider)
    {
    }
}
