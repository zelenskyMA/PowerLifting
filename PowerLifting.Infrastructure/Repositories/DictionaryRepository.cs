using PowerLifting.Domain.DbModels;
using PowerLifting.Infrastructure.Common;
using PowerLifting.Infrastructure.DataContext;

namespace PowerLifting.Infrastructure.Repositories
{
    public class DictionaryRepository : CrudRepo<DictionaryDb>
    {
        public DictionaryRepository(IContextProvider provider) : base(provider) { }
    }
}
