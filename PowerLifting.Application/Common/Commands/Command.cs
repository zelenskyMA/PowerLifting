using PowerLifting.Infrastructure.Setup;

namespace SportAssist.Common.Commands
{
    public class Command<TParam, TResult> : ICommand<TParam, TResult>
    {
        private readonly Func<ICommand<TParam, TResult>> _commandAccessor;
        private readonly IContextProvider _provider;

        /// <summary> Initializes a new instance of the <see cref="Command{TParam, TResult}"/> class. </summary>
        /// <param name="commandAccessor"><see cref="ICommand{TParam,TResult}"/></param>
        /// <param name="provider"><see cref="IContextProvider"/></param>
        public Command(Func<ICommand<TParam, TResult>> commandAccessor, IContextProvider provider)
        {
            _commandAccessor = commandAccessor;
            _provider = provider;
        }

        /// <inheritdoc />
        public async Task<TResult> ExecuteAsync(TParam param)
        {
            _provider.BeginTransaction();

            TResult result = await _commandAccessor().ExecuteAsync(param);

            _provider.CommitTransaction();

            return result;
        }
    }
}
