using Microsoft.EntityFrameworkCore;
using PowerLifting.Domain.Interfaces.Repositories;
using System.Linq.Expressions;

namespace PowerLifting.Infrastructure.Repositories.Common
{
  public class CrudRepo<T> : BaseRepo<T>, ICrudRepo<T> where T : class
  {
    public CrudRepo(DbContextOptions<LiftingContext> provider) : base(provider) { }


    public IEnumerable<T> Find(Expression<Func<T, bool>> predicate) => DbSet.Where(predicate).ToList();

    public virtual async Task<T> Get(string name) => await DbSet.FindAsync(name);

    public async Task Delete(string name)
    {
      var entity = await Get(name);
      if (entity == null) return; // not found; assume already deleted.
      await DeleteAsync(entity);
    }
  }
}
