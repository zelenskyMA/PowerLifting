using Microsoft.EntityFrameworkCore;
using PowerLifting.Domain.Interfaces.Repositories;
using System.Linq.Expressions;

namespace PowerLifting.Infrastructure.Repositories.Common
{
  public class CrudRepo<T> : BaseRepo<T>, ICrudRepo<T> where T : class
  {
    public CrudRepo(DbContextOptions<LiftingContext> provider) : base(provider) { }

    public Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate) =>
      predicate == null ? DbSet.ToListAsync() : DbSet.Where(predicate).ToListAsync();

    public virtual async Task<T> GetAsyn(string name) => await DbSet.FindAsync(name);

    public async Task DeleteAsync(string name)
    {
      var entity = await GetAsyn(name);
      if (entity == null) return; // not found; assume already deleted.
      await DeleteAsync(entity);
    }
  }
}
