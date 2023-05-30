using Microsoft.AspNetCore.Mvc;
using SportAssistant.Application.TrainingPlan.PlanCommands;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Models.TrainingPlan;

namespace SportAssistant.Service.Controllers.TrainingPlan;

[Route("trainingPlan")]
public class PlanController : BaseController
{
    [HttpGet]
    [Route("{id}")]
    public async Task<Plan> GetAsync([FromServices] ICommand<PlanGetByIdQuery.Param, Plan> command, int id)
    {
        var result = await command.ExecuteAsync(new PlanGetByIdQuery.Param() { Id = id });
        return result;
    }

    [HttpGet]
    [Route("getList/{userId}")]
    public async Task<Plans> GetListAsync([FromServices] ICommand<PlanGetListQuery.Param, Plans> command, int userId = 0)
    {
        var result = await command.ExecuteAsync(new PlanGetListQuery.Param() { UserId = userId });
        return result;
    }

    [HttpPost]
    public async Task<int> CreateAsync([FromServices] ICommand<PlanCreateCommand.Param, int> command, PlanCreateCommand.Param param)
    {
        var result = await command.ExecuteAsync(param);
        return result;
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<bool> DeleteAsync([FromServices] ICommand<PlanDeleteCommand.Param, bool> command, int id)
    {
        var result = await command.ExecuteAsync(new PlanDeleteCommand.Param() { Id = id });
        return result;
    }
}
