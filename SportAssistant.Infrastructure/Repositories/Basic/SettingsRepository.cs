using SportAssistant.Domain.DbModels.Basic;
using SportAssistant.Infrastructure.Common;
using SportAssistant.Infrastructure.DataContext;

namespace SportAssistant.Infrastructure.Repositories.Basic;

public class SettingsRepository : CrudRepo<SettingsDb>
{
    public SettingsRepository(IContextProvider provider) : base(provider) { }
}
