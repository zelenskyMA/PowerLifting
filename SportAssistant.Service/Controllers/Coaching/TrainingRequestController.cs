using LoggerLib.Middleware;
using Microsoft.AspNetCore.Mvc;
using SportAssistant.Application.Coaching.TrainingRequestCommands;
using SportAssistant.Application.UserData.Auth.Interfaces;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Models.Coaching;

namespace SportAssistant.Service.Controllers.Coaching;


[Route("trainingRequests")]
public class TrainingRequestController : BaseController
{
    private readonly IUserProvider _user;

    public TrainingRequestController(IUserProvider user)
    {
        _user = user;
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<TrainingRequest> GetAsync([FromServices] ICommand<RequestGetForCoachSingleQuery.Param, TrainingRequest> command, int id)
    {
        var result = await command.ExecuteAsync(new RequestGetForCoachSingleQuery.Param() { Id = id });
        return result;
    }

    [HttpGet]
    [Route("getMyRequest")]
    public async Task<TrainingRequest> GetMyRequestAsync([FromServices] ICommand<RequestGetByUserQuery.Param, TrainingRequest> command)
    {
        var result = await command.ExecuteAsync(new RequestGetByUserQuery.Param() { UserId = _user.Id });
        return result;
    }

    [HttpGet]
    [Route("getCoachRequests"), ExcludeLogItem]
    public async Task<List<TrainingRequest>> GetCoachRequestsAsync([FromServices] ICommand<RequestGetForCoachQuery.Param, List<TrainingRequest>> command)
    {
        var result = await command.ExecuteAsync(new RequestGetForCoachQuery.Param() { });
        return result;
    }

    [HttpGet]
    [Route("getCoaches"), ExcludeLogItem]
    public async Task<List<CoachInfo>> GetCoachesAsync([FromServices] ICommand<RequestGetAvailableCoachesQuery.Param, List<CoachInfo>> command)
    {
        var result = await command.ExecuteAsync(new RequestGetAvailableCoachesQuery.Param() { });
        return result;
    }

    [HttpPost]
    [Route("{coachId}")]
    public async Task<bool> CreateAsync([FromServices] ICommand<RequestCreateCommand.Param, bool> command, int coachId)
    {
        var result = await command.ExecuteAsync(new RequestCreateCommand.Param() { СoachId = coachId });
        return result;
    }

    [HttpDelete]
    [Route("{userId?}")]
    public async Task<bool> RemoveAsync([FromServices] ICommand<RequestRemoveCommand.Param, bool> command, int userId = 0)
    {
        var result = await command.ExecuteAsync(new RequestRemoveCommand.Param() { UserId = userId });
        return result;
    }
}
