using Microsoft.AspNetCore.Mvc;
using SportAssistant.Application.TraininTemplate.TemplatePlanCommands;
using SportAssistant.Application.TraininTemplate.TemplateSetCommands;
using SportAssistant.Domain.DbModels.TraininTemplate;
using SportAssistant.Domain.Interfaces.Common.Operations;

namespace SportAssistant.Service.Controllers.TraininTemplate
{
    [Route("templatePlan")]
    public class TemplatePlanController : BaseController
    {
        [HttpGet]
        [Route("{id}")]
        public async Task<TemplatePlan> GetAsync([FromServices] ICommand<TemplatePlanGetByIdQuery.Param, TemplatePlan> command, int id)
        {
            var result = await command.ExecuteAsync(new TemplatePlanGetByIdQuery.Param() { Id = id });
            return result;
        }

        [HttpPost]
        public async Task<int> CreateAsync([FromServices] ICommand<TemplatePlanCreateCommand.Param, int> command, TemplatePlanCreateCommand.Param param)
        {
            var result = await command.ExecuteAsync(param);
            return result;
        }

        [HttpPut]
        public async Task<int> UpdateAsync([FromServices] ICommand<TemplatePlanUpdateCommand.Param, int> command, TemplatePlan templatePlan)
        {
            var result = await command.ExecuteAsync(new TemplatePlanUpdateCommand.Param() { TemplatePlan = templatePlan });
            return result;
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<int> DeleteAsync([FromServices] ICommand<TemplatePlanDeleteCommand.Param, int> command, int id)
        {
            var result = await command.ExecuteAsync(new TemplatePlanDeleteCommand.Param() { Id = id });
            return result;
        }
    }
}