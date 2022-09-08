using System.Linq.Expressions;

namespace PowerLifting.Domain.Interfaces.Common.Repositories
{
    public interface ICrudRepo<T> : IBaseRepo<T> where T : class
    {
        Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate);
    }
}
