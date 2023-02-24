using Microsoft.AspNetCore.Mvc;
using SportAssistant.Application.TrainingTemplate.TemplateSetCommands;
using SportAssistant.Domain.DbModels.TrainingTemplate;
using SportAssistant.Domain.Interfaces.Common.Operations;

namespace SportAssistant.Service.Controllers.TrainingTemplate
{
    [Route("templateSet")]
    public class TemplateSetController : BaseController
    {
        [HttpGet]
        [Route("{id}")]
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
        public async Task<int> CreateAsync([FromServices] ICommand<TemplateSetCreateCommand.Param, int> command, TemplateSetCreateCommand.Param param)
        {
            var result = await command.ExecuteAsync(param);
            return result;
        }

        [HttpPost]
        [Route("assign")]
        public async Task<bool> AssignAsync([FromServices] ICommand<TemplateSetAssignCommand.Param, bool> command, TemplateSetAssignCommand.Param param)
        {
            var result = await command.ExecuteAsync(param);
            return result;
        }

        [HttpPut]
        public async Task<bool> UpdateAsync([FromServices] ICommand<TemplateSetUpdateCommand.Param, bool> command, TemplateSet templateSet)
        {
            var result = await command.ExecuteAsync(new TemplateSetUpdateCommand.Param() { TemplateSet = templateSet });
            return result;
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<bool> DeleteAsync([FromServices] ICommand<TemplateSetDeleteCommand.Param, bool> command, int id)
        {
            var result = await command.ExecuteAsync(new TemplateSetDeleteCommand.Param() { Id = id });
            return result;
        }
    }
}
