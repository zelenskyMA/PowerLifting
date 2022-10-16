using SportAssistant.Domain.DbModels;
using SportAssistant.Infrastructure.Common;
using SportAssistant.Infrastructure.DataContext;

namespace SportAssistant.Infrastructure.Repositories
{
    public class DictionaryRepository : CrudRepo<DictionaryDb>
    {
        public DictionaryRepository(IContextProvider provider) : base(provider) { }
    }
}
