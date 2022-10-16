namespace PowerLifting.Domain.Interfaces.Common.Operations
{
    public interface ICommand<TParam, TResult>
    {
        /// <summary>
        /// Выполнение команды
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<TResult> ExecuteAsync(TParam param);
    }
}
