using Microsoft.EntityFrameworkCore;
using PowerLifting.Domain.DbModels;
using PowerLifting.Infrastructure.Repositories.Common;

namespace PowerLifting.Infrastructure.Repositories
{
    public class DictionaryTypeRepository : CrudRepo<DictionaryTypeDb>
    {
        public DictionaryTypeRepository(DbContextOptions<LiftingContext> provider) : base(provider)
        {
        }
    }
}
