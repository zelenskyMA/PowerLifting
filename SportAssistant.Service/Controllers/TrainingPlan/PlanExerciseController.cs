using Microsoft.AspNetCore.Mvc;
using SportAssistant.Application.TrainingPlan.PlanExerciseCommands;
using SportAssistant.Application.TrainingPlan.PlanExerciseSettingsCommands;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Models.TrainingPlan;

namespace SportAssistant.Service.Controllers.TrainingPlan
{
    [Route("planExercise")]
    public class PlanExerciseController : BaseController
    {
        [HttpGet]
        [Route("get")]
        public async Task<PlanExercise> GetAsync([FromServices] ICommand<PlanExerciseGetByIdQuery.Param, PlanExercise> command, int id)
        {
            var result = await command.ExecuteAsync(new PlanExerciseGetByIdQuery.Param() { Id = id });
            return result;
        }

        [HttpGet]
        [Route("getByDay")]
        public async Task<List<PlanExercise>> GetByDay([FromServices] ICommand<PlanExerciseGetByDaysQuery.Param, List<PlanExercise>> command, int dayId)
        {
            var result = await command.ExecuteAsync(new PlanExerciseGetByDaysQuery.Param() { DayIds = new List<int>() { dayId } });
            return result;
        }

        [HttpPost]
        [Route("create")]
        public async Task<bool> CreateAsync([FromServices] ICommand<PlanExerciseCreateCommand.Param, bool> command, PlanExerciseCreateCommand.Param param)
        {
            var result = await command.ExecuteAsync(param);
            return result;
        }

        [HttpPost]
        [Route("update")]
        public async Task<bool> UpdateAsync([FromServices] ICommand<PlanExerciseUpdateCommand.Param, bool> command, PlanExerciseUpdateCommand.Param param)
        {
            var result = await command.ExecuteAsync(param);
            return result;
        }

        [HttpPost]
        [Route("complete")]
        public async Task<bool> CompleteAsync([FromServices] ICommand<PlanExerciseSettingsComplete.Param, bool> command, List<int> exerciseIds)
        {
            var result = await command.ExecuteAsync(new PlanExerciseSettingsComplete.Param() { Ids = exerciseIds });
            return result;
        }
    }
}
