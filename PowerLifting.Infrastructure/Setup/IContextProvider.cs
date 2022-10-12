using Microsoft.EntityFrameworkCore.Storage;

namespace PowerLifting.Infrastructure.Setup
{
    public interface IContextProvider
    {
        LiftingContext Context { get; }

        IDbContextTransaction BeginTransaction();

        IDbContextTransaction GetTransaction();

        Task CommitTransactionAsync();
    }
}
