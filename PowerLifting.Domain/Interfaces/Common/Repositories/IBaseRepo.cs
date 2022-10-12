namespace PowerLifting.Domain.Interfaces.Common.Repositories
{
    public interface IBaseRepo<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();

        Task<bool> CreateAsync(T entity);

        bool Update(T entity);

        bool Delete(T entity);
    }
}
