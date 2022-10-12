using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace PowerLifting.Infrastructure.Setup
{
    public class SqlContextProvider : IContextProvider
    {
        private bool _disposed = false;
        private IDbContextTransaction _transaction;

        public LiftingContext Context { get; }

        public SqlContextProvider(DbContextOptions<LiftingContext> contextOptions)
        {
            Context = new LiftingContext(contextOptions);
        }

        /// <inheritdoc/>
        public IDbContextTransaction BeginTransaction()
        {
            _transaction = Context.Database.CurrentTransaction ?? Context.Database.BeginTransaction();
            return _transaction;
        }

        /// <inheritdoc/>
        public IDbContextTransaction GetTransaction()
        {
            if (_transaction is null)
            {
                throw new InvalidOperationException("Отсутствует открытая транзакция.");
            }

            return _transaction;
        }

        /// <inheritdoc/>
        public async Task CommitTransactionAsync()
        {
            if (Context.Database.CurrentTransaction == null)
            {
                throw new InvalidOperationException("Отсутствует открытая транзакция.");
            }

            await Context.Database.CommitTransactionAsync();
            await Context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Выполняет освобождение ресурсов.
        /// </summary>
        /// <param name="disposing">Признак освобождения управляемых ресурсов.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    DisposeObject(_transaction);
                    Context.Dispose();
                }

                _disposed = true;
            }
        }

        private void DisposeObject(IDisposable obj)
        {
            try
            {
                obj?.Dispose();
            }
            catch { }
        }
    }

}
