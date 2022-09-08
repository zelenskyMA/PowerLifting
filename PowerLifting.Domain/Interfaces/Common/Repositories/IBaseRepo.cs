namespace PowerLifting.Domain.Interfaces.Common.Repositories
{
    public interface IBaseRepo<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();

        Task CreateAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteAsync(T entity);

        Task DeleteListAsync(List<T> entities);
    }
}
