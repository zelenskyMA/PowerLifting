using Microsoft.EntityFrameworkCore;
using PowerLifting.Domain.Interfaces.Common.Repositories;
using PowerLifting.Infrastructure.DataContext;

namespace PowerLifting.Infrastructure.Common
{
    public class BaseRepo<T> : IBaseRepo<T> where T : class
    {
        protected SportContext Context { get; }
        protected DbSet<T> DbSet { get; set; }

        public BaseRepo(IContextProvider provider)
        {
            Context = provider.Context;
            DbSet = Context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync() => await DbSet.AsNoTracking().ToListAsync();

        public async Task CreateAsync(T entity)
        {
            var dbEntityEntry = Context.Entry(entity);
            if (dbEntityEntry.State != EntityState.Detached)
            {
                dbEntityEntry.State = EntityState.Added;
            }
            else
            {
                await DbSet.AddAsync(entity);
            }
        }

        public void Update(T entity)
        {
            var dbEntityEntry = Context.Entry(entity);
            if (dbEntityEntry.State == EntityState.Detached)
            {
                DbSet.Attach(entity);
            }
            dbEntityEntry.State = EntityState.Modified;
        }

        public void Delete(T entity)
        {
            var dbEntityEntry = Context.Entry(entity);
            dbEntityEntry.State = EntityState.Deleted;
            DbSet.Remove(entity);
        }
    }
}
