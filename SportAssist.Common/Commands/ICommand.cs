namespace SportAssist.Common.Commands
{
    public interface ICommand<TParam, TResult>
    {
        Task<TResult> ExecuteAsync(TParam param);
    }
}
