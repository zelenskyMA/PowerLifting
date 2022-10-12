using Microsoft.EntityFrameworkCore;
using PowerLifting.Domain.Interfaces.Common.Repositories;
using PowerLifting.Infrastructure.Setup;
using System.Linq.Expressions;

namespace PowerLifting.Infrastructure.Repositories.Common
{
    public class CrudRepo<T> : BaseRepo<T>, ICrudRepo<T> where T : class
    {
        public CrudRepo(IContextProvider provider) : base(provider) { }

        public Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate) =>
          predicate == null ? DbSet.ToListAsync() : DbSet.Where(predicate).ToListAsync();

    }
}
