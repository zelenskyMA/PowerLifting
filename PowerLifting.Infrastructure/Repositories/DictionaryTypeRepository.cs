using PowerLifting.Domain.DbModels;
using PowerLifting.Infrastructure.Repositories.Common;
using PowerLifting.Infrastructure.Setup;

namespace PowerLifting.Infrastructure.Repositories
{
    public class DictionaryTypeRepository : CrudRepo<DictionaryTypeDb>
    {
        public DictionaryTypeRepository(IContextProvider provider) : base(provider)
        {
        }
    }
}
