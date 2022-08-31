using Microsoft.EntityFrameworkCore;
using PowerLifting.Domain.Interfaces.Repositories;

namespace PowerLifting.Infrastructure.Repositories.Common
{
  public class BaseRepo<T> : IBaseRepo<T>, IDisposable where T : class
    {
        protected LiftingContext Context { get; }
        protected DbSet<T> DbSet { get; set; }

        public BaseRepo(DbContextOptions<LiftingContext> provider)
        {
            Context = new LiftingContext(provider);
            DbSet = Context.Set<T>();
        }


        public IEnumerable<T> GetAll() => DbSet.ToList();

        public async Task Create(T entity)
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

            await Context.SaveChangesAsync();
        }

        public async Task Update(T entity)
        {
            var dbEntityEntry = Context.Entry(entity);
            if (dbEntityEntry.State == EntityState.Detached)
            {
                DbSet.Attach(entity);
            }
            dbEntityEntry.State = EntityState.Modified;

            await Context.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            var dbEntityEntry = Context.Entry(entity);
            dbEntityEntry.State = EntityState.Deleted;
            DbSet.Remove(entity);

            await Context.SaveChangesAsync();
        }


        public void Dispose() { }// => Context?.Dispose();
    }
}
