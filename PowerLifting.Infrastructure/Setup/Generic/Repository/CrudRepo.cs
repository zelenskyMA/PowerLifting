using Microsoft.EntityFrameworkCore;
using PowerLifting.Domain.Interfaces.Common.Repositories;
using PowerLifting.Domain.Models.Common;
using System.Linq.Expressions;

namespace PowerLifting.Infrastructure.Setup.Generic.Repository
{
    public class CrudRepo<T> : BaseRepo<T>, ICrudRepo<T> where T : class
    {
        public CrudRepo(IContextProvider provider) : base(provider) { }

        public Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate) =>
          predicate == null ? DbSet.AsNoTracking().ToListAsync() : DbSet.AsNoTracking().Where(predicate).ToListAsync();

        public Task<T?> FindOneAsync(Expression<Func<T, bool>> predicate) =>
            predicate == null ? DbSet.AsNoTracking().FirstOrDefaultAsync() : DbSet.AsNoTracking().FirstOrDefaultAsync(predicate);

        public async Task CreateListAsync(List<T> entities) => entities.Select(async t => await CreateAsync(t));

        public void DeleteList(List<T> entities) => entities.Select(t => Delete(t));
        
        public void UpdateList(List<T> entities) => entities.Select(t => Update(t));
    }
}
