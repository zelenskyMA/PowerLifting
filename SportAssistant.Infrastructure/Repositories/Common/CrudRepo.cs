using Microsoft.EntityFrameworkCore;
using SportAssistant.Domain.Interfaces.Common.Repositories;
using SportAssistant.Infrastructure.DataContext;
using System.Linq.Expressions;

namespace SportAssistant.Infrastructure.Common;

public class CrudRepo<T> : BaseRepo<T>, ICrudRepo<T> where T : class
{
    public CrudRepo(IContextProvider provider) : base(provider) { }

    public Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate) =>
      predicate == null ? DbSet.AsNoTracking().ToListAsync() : DbSet.AsNoTracking().Where(predicate).ToListAsync();

    public Task<T?> FindOneAsync(Expression<Func<T, bool>> predicate) =>
        predicate == null ? DbSet.AsNoTracking().FirstOrDefaultAsync() : DbSet.AsNoTracking().FirstOrDefaultAsync(predicate);

    public async Task CreateListAsync(List<T> entities)
    {
        foreach (var item in entities)
        {
            await CreateAsync(item);
        }
    }

    public void DeleteList(List<T> entities)
    {
        foreach (var item in entities)
        {
            Delete(item);
        }
    }

    public void UpdateList(List<T> entities)
    {
        foreach (var item in entities)
        {
            Update(item);
        }
    }
}
