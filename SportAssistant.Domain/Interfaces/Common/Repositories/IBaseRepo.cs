namespace SportAssistant.Domain.Interfaces.Common.Repositories;

public interface IBaseRepo<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();

    Task CreateAsync(T entity);

    void Update(T entity);

    void Delete(T entity);
}
