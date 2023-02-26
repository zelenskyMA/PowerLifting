using Microsoft.AspNetCore.Mvc;
using SportAssistant.Application.TrainingTemplate.TemplateDayCommands;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Models.TrainingTemplate;

namespace SportAssistant.Service.Controllers.TrainingTemplate
{

    [Route("templateDay")]
    public class TemplateDayController : BaseController
    {
        [HttpGet]
        [Route("{id}")]
        public async Task<TemplateDay> GetAsync([FromServices] ICommand<TemplateDayGetByIdQuery.Param, TemplateDay> command, int id)
        {
            var result = await command.ExecuteAsync(new TemplateDayGetByIdQuery.Param() { Id = id });
            return result;
        }
    }
}
