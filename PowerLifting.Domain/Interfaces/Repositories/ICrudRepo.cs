using System.Linq.Expressions;

namespace PowerLifting.Domain.Interfaces.Repositories
{
  public interface ICrudRepo<T> : IBaseRepo<T> where T : class
  {
    IEnumerable<T> Find(Expression<Func<T, bool>> predicate);

    Task<T> Get(string name);

    Task Delete(string name);
  }
}
