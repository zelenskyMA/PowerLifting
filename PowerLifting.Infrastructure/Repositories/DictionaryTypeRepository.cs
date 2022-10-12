using PowerLifting.Domain.DbModels;
using PowerLifting.Infrastructure.Setup;
using PowerLifting.Infrastructure.Setup.Generic.Repository;

namespace PowerLifting.Infrastructure.Repositories
{
    public class DictionaryTypeRepository : CrudRepo<DictionaryTypeDb>
    {
        public DictionaryTypeRepository(IContextProvider provider) : base(provider)
        {
        }
    }
}
