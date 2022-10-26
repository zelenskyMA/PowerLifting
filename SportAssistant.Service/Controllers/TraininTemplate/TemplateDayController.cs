using Microsoft.AspNetCore.Mvc;
using SportAssistant.Application.TraininTemplate.TemplateDayCommands;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Models.TraininTemplate;

namespace SportAssistant.Service.Controllers.TraininTemplate
{

    [Route("templateDay")]
    public class TemplateDayController : BaseController
    {
        [HttpGet]
        [Route("get")]
        public async Task<TemplateDay> GetAsync([FromServices] ICommand<TemplateDayGetByIdQuery.Param, TemplateDay> command, int id)
        {
            var result = await command.ExecuteAsync(new TemplateDayGetByIdQuery.Param() { Id = id });
            return result;
        }
    }
}
