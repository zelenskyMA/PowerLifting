using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Infrastructure.DataContext;

namespace SportAssistant.Infrastructure.Setup.Generic.AppActions;

public class Command<TParam, TResult> : ICommand<TParam, TResult>
{
    private readonly Func<ICommand<TParam, TResult>> _commandAccessor;
    private readonly IContextProvider _provider;

    public Command(Func<ICommand<TParam, TResult>> commandAccessor, IContextProvider provider)
    {
        _commandAccessor = commandAccessor;
        _provider = provider;
    }

    /// <inheritdoc />
    public async Task<TResult> ExecuteAsync(TParam param)
    {
        try
        {
            _provider.BeginTransaction();

            TResult result = await _commandAccessor().ExecuteAsync(param);

            await _provider.CommitTransactionAsync();

            return result;
        }
        catch (Exception)
        {
            await _provider.RollbackAsync();
            throw;
        }
    }
}
