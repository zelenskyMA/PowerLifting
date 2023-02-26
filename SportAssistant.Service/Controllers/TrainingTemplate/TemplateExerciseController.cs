using Microsoft.AspNetCore.Mvc;
using SportAssistant.Application.TrainingTemplate.TemplateExerciseCommands;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Models.TrainingTemplate;

namespace SportAssistant.Service.Controllers.TrainingTemplate
{
    [Route("templateExercise")]
    public class TemplateExerciseController : BaseController
    {
        [HttpGet]
        [Route("{id}")]
        public async Task<TemplateExercise> GetAsync([FromServices] ICommand<TemplateExerciseGetByIdQuery.Param, TemplateExercise> command, int id)
        {
            var result = await command.ExecuteAsync(new TemplateExerciseGetByIdQuery.Param() { Id = id });
            return result;
        }

        [HttpGet]
        [Route("getByDay/{dayId}")]
        public async Task<List<TemplateExercise>> GetByDay([FromServices] ICommand<TemplateExerciseGetByDaysQuery.Param, List<TemplateExercise>> command, int dayId)
        {
            var result = await command.ExecuteAsync(new TemplateExerciseGetByDaysQuery.Param() { DayId = dayId });
            return result;
        }

        [HttpPost]
        public async Task<bool> CreateAsync([FromServices] ICommand<TemplateExerciseCreateCommand.Param, bool> command, TemplateExerciseCreateCommand.Param param)
        {
            var result = await command.ExecuteAsync(param);
            return result;
        }

        [HttpPut]
        public async Task<bool> UpdateAsync([FromServices] ICommand<TemplateExerciseUpdateCommand.Param, bool> command, TemplateExerciseUpdateCommand.Param param)
        {
            var result = await command.ExecuteAsync(param);
            return result;
        }
    }
}
