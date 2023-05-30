using SportAssistant.Domain.DbModels.Basic;
using SportAssistant.Infrastructure.Common;
using SportAssistant.Infrastructure.DataContext;

namespace SportAssistant.Infrastructure.Repositories.Basic;

public class DictionaryRepository : CrudRepo<DictionaryDb>
{
    public DictionaryRepository(IContextProvider provider) : base(provider) { }
}
