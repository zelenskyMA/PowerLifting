using LoggerLib.Middleware;
using Microsoft.AspNetCore.Mvc;
using SportAssistant.Application.Management.ManagerCommands;
using SportAssistant.Domain.DbModels.Management;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Service.Controllers;

[Route("manager")]
public class ManagerController : BaseController
{
    [HttpGet]
    [Route("getList/{orgId}"), ExcludeLogItem]
    public async Task<List<Manager>> GetListAsync([FromServices] ICommand<ManagerGetListQuery.Param, List<Manager>> command, int orgId)
    {
        var result = await command.ExecuteAsync(new ManagerGetListQuery.Param() { OrgId = orgId });
        return result;
    }


    [HttpPost]
    [Route("changeRole")]
    public async Task<bool> ManagerRoleChangeAsync([FromServices] ICommand<ManagerRoleChangeCommand.Param, bool> command, ManagerRoleChangeCommand.Param param)
    {
        var result = await command.ExecuteAsync(param);
        return result;
    }
}
