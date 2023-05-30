using SportAssistant.Domain.DbModels.Basic;
using SportAssistant.Infrastructure.Common;
using SportAssistant.Infrastructure.DataContext;

namespace SportAssistant.Infrastructure.Repositories.Basic;

public class DictionaryTypeRepository : CrudRepo<DictionaryTypeDb>
{
    public DictionaryTypeRepository(IContextProvider provider) : base(provider)
    {
    }
}
