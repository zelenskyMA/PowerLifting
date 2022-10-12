using PowerLifting.Domain.DbModels;
using PowerLifting.Infrastructure.Setup;
using PowerLifting.Infrastructure.Setup.Generic.Repository;

namespace PowerLifting.Infrastructure.Repositories
{
    public class DictionaryRepository : CrudRepo<DictionaryDb>
    {
        public DictionaryRepository(IContextProvider provider) : base(provider) { }
    }
}
