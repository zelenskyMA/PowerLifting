using Microsoft.AspNetCore.Mvc;
using SportAssistant.Application.TraininTemplate.TemplateSetCommands;
using SportAssistant.Domain.DbModels.TraininTemplate;
using SportAssistant.Domain.Interfaces.Common.Operations;

namespace SportAssistant.Service.Controllers.TraininTemplate
{
    [Route("templateSet")]
    public class TemplateSetController : BaseController
    {
        [HttpGet]
        [Route("get")]
        public async Task<TemplateSet> GetAsync([FromServices] ICommand<TemplateSetGetByIdQuery.Param, TemplateSet> command, int id)
        {
            var result = await command.ExecuteAsync(new TemplateSetGetByIdQuery.Param() { Id = id });
            return result;
        }

        [HttpGet]
        [Route("getList")]
        public async Task<List<TemplateSet>> GetListAsync([FromServices] ICommand<TemplateSetGetListQuery.Param, List<TemplateSet>> command)
        {
            var result = await command.ExecuteAsync(new TemplateSetGetListQuery.Param() { });
            return result;
        }

        [HttpPost]
        [Route("create")]
        public async Task<int> CreateAsync([FromServices] ICommand<TemplateSetCreateCommand.Param, int> command, TemplateSetCreateCommand.Param param)
        {
            var result = await command.ExecuteAsync(param);
            return result;
        }

        [HttpPost]
        [Route("update")]
        public async Task<bool> UpdateAsync([FromServices] ICommand<TemplateSetUpdateCommand.Param, bool> command, TemplateSet templateSet)
        {
            var result = await command.ExecuteAsync(new TemplateSetUpdateCommand.Param() { TemplateSet = templateSet });
            return result;
        }

        [HttpPost]
        [Route("delete")]
        public async Task<bool> DeleteAsync([FromServices] ICommand<TemplateSetDeleteCommand.Param, bool> command, TemplateSetDeleteCommand.Param param)
        {
            var result = await command.ExecuteAsync(param);
            return result;
        }
    }
}
