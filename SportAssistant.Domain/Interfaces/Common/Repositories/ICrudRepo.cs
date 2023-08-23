using System.Linq.Expressions;

namespace SportAssistant.Domain.Interfaces.Common.Repositories;

public interface ICrudRepo<T> : IBaseRepo<T> where T : class
{
    Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate);

    Task<T?> FindOneAsync(Expression<Func<T, bool>> predicate);

    Task<int> CountAsync(Expression<Func<T, bool>> predicate);

    Task CreateListAsync(List<T> entities);

    void DeleteList(List<T> entities);

    void UpdateList(List<T> entities);
}
