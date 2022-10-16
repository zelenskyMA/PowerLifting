using PowerLifting.Domain.DbModels;
using PowerLifting.Infrastructure.Common;
using PowerLifting.Infrastructure.DataContext;

namespace PowerLifting.Infrastructure.Repositories
{
    public class DictionaryTypeRepository : CrudRepo<DictionaryTypeDb>
    {
        public DictionaryTypeRepository(IContextProvider provider) : base(provider)
        {
        }
    }
}
