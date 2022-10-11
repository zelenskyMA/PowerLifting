using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportAssist.Common.Commands
{
    //public class Command<TParam, TResult> : ICommand<TParam, TResult>
    //{
    //    private readonly ICommand<TParam, TResult> _command;
    //    private readonly ISimpleConnectionFactory _connectionFactory;

    //    /// <summary> Initializes a new instance of the <see cref="Command{TParam, TResult}"/> class. </summary>
    //    /// <param name="command"><see cref="ICommand{TParam,TResult}"/></param>
    //    /// <param name="connectionFactory"><see cref="ISimpleConnectionFactory"/></param>
    //    public Command(ICommand<TParam, TResult> command, ISimpleConnectionFactory connectionFactory)
    //    {
    //        _command = command;
    //        _connectionFactory = connectionFactory;
    //    }

    //    /// <inheritdoc />
    //    public async Task<TResult> ExecuteAsync(TParam param)
    //    {
    //        _connectionFactory.OpenConnection();
    //        using var tran = _connectionFactory.BeginTransaction();

    //        TResult result = await _command.ExecuteAsync(param);

    //        tran.Commit();

    //        return result;
    //    }
    //}
}
