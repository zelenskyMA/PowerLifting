using LoggerLib.Middleware;
using Microsoft.AspNetCore.Mvc;
using SportAssistant.Application.Management.OrganizationCommands;
using SportAssistant.Domain.DbModels.Management;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Models.Management;

namespace SportAssistant.Service.Controllers.Management;

[Route("organization")]
public class OrganizationController : BaseController
{
    [HttpGet]
    [Route("getList"), ExcludeLogItem]
    public async Task<List<Organization>> GetListAsync([FromServices] ICommand<OrgGetListQuery.Param, List<Organization>> command)
    {
        var result = await command.ExecuteAsync(new OrgGetListQuery.Param() { });
        return result;
    }


    [HttpGet]
    [Route("")]
    [Route("{id}")]
    public async Task<Organization> GetAsync([FromServices] ICommand<OrgGetByIdQuery.Param, Organization> command, int id)
    {
        var result = await command.ExecuteAsync(new OrgGetByIdQuery.Param() { Id = id });
        return result;
    }

    [HttpGet]
    [Route("info"), ExcludeLogItem]
    public async Task<OrgInfo> GetListAsync([FromServices] ICommand<OrgInfoGetQuery.Param, OrgInfo> command)
    {
        var result = await command.ExecuteAsync(new OrgInfoGetQuery.Param());
        return result;
    }

    /// <summary>
    /// Создание идет админом, а обновление - владельцем или админом. Чтобы не плодить иф, проще иметь 2 эндпоинта.
    /// </summary>
    /// <param name="command"><see cref="OrgCreateCommand"/></param>
    /// <param name="org">Новая организация</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<bool> CreateAsync([FromServices] ICommand<OrgCreateCommand.Param, bool> command, Organization org)
    {
        var result = await command.ExecuteAsync(new OrgCreateCommand.Param() { Organization = org });
        return result;
    }

    [HttpPut]
    public async Task<bool> UpdateAsync([FromServices] ICommand<OrgUpdateCommand.Param, bool> command, Organization org)
    {
        var result = await command.ExecuteAsync(new OrgUpdateCommand.Param() { Organization = org });
        return result;
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<bool> DeleteAsync([FromServices] ICommand<OrgDeleteCommand.Param, bool> command, int id)
    {
        var result = await command.ExecuteAsync(new OrgDeleteCommand.Param() { Id = id });
        return result;
    }
}
