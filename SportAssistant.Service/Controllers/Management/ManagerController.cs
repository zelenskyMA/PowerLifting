using LoggerLib.Middleware;
using Microsoft.AspNetCore.Mvc;
using SportAssistant.Application.Management.ManagerCommands;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Models.Management;
using SportAssistant.Service.Controllers;

[Route("manager")]
public class ManagerController : BaseController
{
    [HttpGet, ExcludeLogItem]
    [Route("getList")]
    [Route("getList/{orgId}")]
    public async Task<List<Manager>> GetListAsync([FromServices] ICommand<ManagerGetListQuery.Param, List<Manager>> command)
    {
        var result = await command.ExecuteAsync(new ManagerGetListQuery.Param() { });
        return result;
    }

    [HttpGet]
    [Route("")]
    [Route("{id}")]
    public async Task<Manager> GetAsync([FromServices] ICommand<ManagerGetQuery.Param, Manager> command, int id)
    {
        var result = await command.ExecuteAsync(new ManagerGetQuery.Param() { Id = id });
        return result;
    }

    [HttpPost]
    public async Task<bool> UpdateAsync([FromServices] ICommand<ManagerUpdateCommand.Param, bool> command, ManagerUpdateCommand.Param param)
    {
        var result = await command.ExecuteAsync(param);
        return result;
    }


    [HttpPost]
    [Route("changeRole")]
    public async Task<bool> ManagerRoleChangeAsync([FromServices] ICommand<ManagerRoleChangeCommand.Param, bool> command, ManagerRoleChangeCommand.Param param)
    {
        var result = await command.ExecuteAsync(param);
        return result;
    }

    [HttpPost]
    [Route("transfer")]
    public async Task<bool> ManagerTrnasferCommandAsync([FromServices] ICommand<ManagerTrnasferCommand.Param, bool> command, ManagerTrnasferCommand.Param param)
    {
        var result = await command.ExecuteAsync(param);
        return result;
    }
}
