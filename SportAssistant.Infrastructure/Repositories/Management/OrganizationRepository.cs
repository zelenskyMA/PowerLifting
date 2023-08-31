using SportAssistant.Domain.DbModels.Management;
using SportAssistant.Infrastructure.Common;
using SportAssistant.Infrastructure.DataContext;

namespace SportAssistant.Infrastructure.Repositories.Management;

public class OrganizationRepository : CrudRepo<OrganizationDb>
{
    public OrganizationRepository(IContextProvider provider) : base(provider)
    {
    }
}
