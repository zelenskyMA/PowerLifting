using Microsoft.AspNetCore.Mvc;
using SportAssistant.Application.TraininTemplate.TemplateExerciseCommands;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Models.TraininTemplate;

namespace SportAssistant.Service.Controllers.TraininTemplate
{
    [Route("templateExercise")]
    public class TemplateExerciseController : BaseController
    {
        [HttpGet]
        [Route("get")]
        public async Task<TemplateExercise> GetAsync([FromServices] ICommand<TemplateExerciseGetByIdQuery.Param, TemplateExercise> command, int id)
        {
            var result = await command.ExecuteAsync(new TemplateExerciseGetByIdQuery.Param() { Id = id });
            return result;
        }

        [HttpGet]
        [Route("getByDay")]
        public async Task<List<TemplateExercise>> GetByDay([FromServices] ICommand<TemplateExerciseGetByDaysQuery.Param, List<TemplateExercise>> command, int dayId)
        {
            var result = await command.ExecuteAsync(new TemplateExerciseGetByDaysQuery.Param() { DayIds = new List<int>() { dayId } });
            return result;
        }

        [HttpPost]
        [Route("create")]
        public async Task<bool> CreateAsync([FromServices] ICommand<TemplateExerciseCreateCommand.Param, bool> command, TemplateExerciseCreateCommand.Param param)
        {
            var result = await command.ExecuteAsync(param);
            return result;
        }

        [HttpPost]
        [Route("update")]
        public async Task<bool> UpdateAsync([FromServices] ICommand<TemplateExerciseUpdateCommand.Param, bool> command, TemplateExerciseUpdateCommand.Param param)
        {
            var result = await command.ExecuteAsync(param);
            return result;
        }
    }
}
