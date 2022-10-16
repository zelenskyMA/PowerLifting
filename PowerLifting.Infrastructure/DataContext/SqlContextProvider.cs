using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace PowerLifting.Infrastructure.DataContext
{
    public class SqlContextProvider : IContextProvider
    {
        private bool _disposed = false;
        private IDbContextTransaction _transaction;

        public SportContext Context { get; }

        public SqlContextProvider(DbContextOptions<SportContext> contextOptions) => Context = new SportContext(contextOptions);

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
            if (_transaction == null)
            {
                throw new InvalidOperationException("Отсутствует открытая транзакция.");
            }

            await Context.SaveChangesAsync();
            await _transaction.CommitAsync();
        }

        /// <inheritdoc/>
        public async Task RollbackAsync() => await _transaction.RollbackAsync();

        /// <inheritdoc/>
        public Task AcceptChangesAsync() => Context.SaveChangesAsync();


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
