using Microsoft.EntityFrameworkCore;
using PowerLifting.Domain.DbModels;
using PowerLifting.Infrastructure.Repositories.Common;

namespace PowerLifting.Infrastructure.Repositories
{
    public class DictionaryRepository : CrudRepo<DictionaryDb>
    {
        public DictionaryRepository(DbContextOptions<LiftingContext> provider) : base(provider) { }
    }
}
