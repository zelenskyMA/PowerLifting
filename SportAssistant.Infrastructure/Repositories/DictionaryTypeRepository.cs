using SportAssistant.Domain.DbModels;
using SportAssistant.Infrastructure.Common;
using SportAssistant.Infrastructure.DataContext;

namespace SportAssistant.Infrastructure.Repositories
{
    public class DictionaryTypeRepository : CrudRepo<DictionaryTypeDb>
    {
        public DictionaryTypeRepository(IContextProvider provider) : base(provider)
        {
        }
    }
}
