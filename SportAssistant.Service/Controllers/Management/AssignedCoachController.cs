using LoggerLib.Middleware;
using Microsoft.AspNetCore.Mvc;
using SportAssistant.Application.Management.CoachAssignment;
using SportAssistant.Domain.DbModels.Management;
using SportAssistant.Domain.Interfaces.Common.Operations;

namespace SportAssistant.Service.Controllers.Management;

[Route("assignedCoach")]
public class AssignedCoachController : BaseController
{
    [HttpGet]
    [Route("getList"), ExcludeLogItem]
    public async Task<List<AssignedCoach>> GetListAsync([FromServices] ICommand<AssignedCoachGetListQuery.Param, List<AssignedCoach>> command)
    {
        var result = await command.ExecuteAsync(new AssignedCoachGetListQuery.Param() { });
        return result;
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<AssignedCoach> GetAsync([FromServices] ICommand<AssignedCoachGetQuery.Param, AssignedCoach> command, int id)
    {
        var result = await command.ExecuteAsync(new AssignedCoachGetQuery.Param() { Id = id });
        return result;
    }

    [HttpPost]
    [Route("changeRole")]
    public async Task<bool> CoachRoleChangeAsync([FromServices] ICommand<AssignedCoachRoleChangeCommand.Param, bool> command, AssignedCoachRoleChangeCommand.Param param)
    {
        var result = await command.ExecuteAsync(param);
        return result;
    }
}
