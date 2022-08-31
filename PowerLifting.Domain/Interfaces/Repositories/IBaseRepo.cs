namespace PowerLifting.Domain.Interfaces.Repositories
{
  public interface IBaseRepo<T> where T : class
  {
    IEnumerable<T> GetAll();

    Task Create(T entity);

    Task Update(T entity);

    Task DeleteAsync(T entity);

  }
}
