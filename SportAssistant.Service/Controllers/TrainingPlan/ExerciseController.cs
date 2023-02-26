using Microsoft.AspNetCore.Mvc;
using SportAssistant.Application.TrainingPlan.ExerciseCommands;
using SportAssistant.Domain.Interfaces.Common;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Models.TrainingPlan;

namespace SportAssistant.Service.Controllers.TrainingPlan
{

    [Route("exerciseInfo")]
    public class ExerciseController : BaseController
    {
        private readonly IAllowedUserIds _allowedUserIds;

        public ExerciseController(IAllowedUserIds allowedUserIds)
        {
            _allowedUserIds = allowedUserIds;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<Exercise> Get([FromServices] ICommand<ExerciseGetByIdQuery.Param, Exercise> command, int id)
        {
            var result = await command.ExecuteAsync(new ExerciseGetByIdQuery.Param() { Id = id });
            return result;
        }

        [HttpGet]
        [Route("getPlanningList")]
        public async Task<List<Exercise>> GetPlanningListAsync([FromServices] ICommand<ExerciseGetByUsersQuery.Param, List<Exercise>> command)
        {
            var result = await command.ExecuteAsync(new ExerciseGetByUsersQuery.Param() { UserIds = _allowedUserIds.MyAndCommon });
            return result;
        }

        [HttpGet]
        [Route("getEditingList")]
        public async Task<List<Exercise>> GetEditingListAsync([FromServices] ICommand<ExerciseGetByUsersQuery.Param, List<Exercise>> command)
        {
            var result = await command.ExecuteAsync(new ExerciseGetByUsersQuery.Param() { UserIds = _allowedUserIds.MyOnly });
            return result;
        }

        [HttpGet]
        [Route("getAdminEditingList")]
        public async Task<List<Exercise>> GetAdminEditingListAsync([FromServices] ICommand<ExerciseGetByUsersQuery.Param, List<Exercise>> command)
        {
            var result = await command.ExecuteAsync(new ExerciseGetByUsersQuery.Param() { UserIds = _allowedUserIds.CommonOnly });
            return result;
        }

        [HttpPost]
        public async Task<bool> UpdateAsync([FromServices] ICommand<ExerciseUpdateCommand.Param, bool> command, Exercise exercise)
        {
            var result = await command.ExecuteAsync(new ExerciseUpdateCommand.Param() { Exercise = exercise });
            return result;
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<bool> DeleteAsync([FromServices] ICommand<ExerciseDeleteCommand.Param, bool> command, int id)
        {
            var result = await command.ExecuteAsync(new ExerciseDeleteCommand.Param() { Id = id });
            return result;
        }
    }
}
