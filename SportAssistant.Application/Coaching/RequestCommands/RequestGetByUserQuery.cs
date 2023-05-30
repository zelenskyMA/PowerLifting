using SportAssistant.Domain.Interfaces.Coaching.Application;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Models.Coaching;

namespace SportAssistant.Application.Coaching.TrainingRequestCommands;

/// <summary>
/// Получение заявки по Ид пользователя.
/// </summary>
public class RequestGetByUserQuery : ICommand<RequestGetByUserQuery.Param, TrainingRequest>
{
    private readonly IProcessRequest _processTrainingRequest;

    public RequestGetByUserQuery(
        IProcessRequest processTrainingRequest)
    {
        _processTrainingRequest = processTrainingRequest;
    }

    public async Task<TrainingRequest> ExecuteAsync(Param param)
    {
        var request = await _processTrainingRequest.GetByUserAsync(param.UserId);
        return request;
    }

    public class Param
    {
        public int UserId { get; set; }
    }
}
