using Microsoft.AspNetCore.Mvc;
using SportAssistant.Application.UserData.UserAchivementCommands;
using SportAssistant.Application.UserData.UserCommands1;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Models.UserData;

namespace SportAssistant.Service.Controllers.UserData
{
    [Route("userAchivement")]
    public class UserAchivementController : BaseController
    {
        [HttpGet]
        public async Task<List<UserAchivement>> GetAsync([FromServices] ICommand<UserAchivementGetQuery.Param, List<UserAchivement>> command)
        {
            var result = await command.ExecuteAsync(new UserAchivementGetQuery.Param());
            return result;
        }

        [HttpGet]
        [Route("getByExercise/{planExerciseId}/{exerciseTypeId}")]
        public async Task<UserAchivement> GetByExerciseAsync(
            [FromServices] ICommand<UserAchivementsGetByExerciseQuery.Param, UserAchivement> command,
            int planExerciseId,
            int exerciseTypeId)
        {
            var result = await command.ExecuteAsync(new UserAchivementsGetByExerciseQuery.Param()
            {
                PlanExerciseId = planExerciseId,
                ExerciseTypeId = exerciseTypeId
            });
            return result;
        }

        [HttpPost]
        public async Task<bool> CreateAsync([FromServices] ICommand<UserAchivementCreateCommand.Param, bool> command, UserAchivementCreateCommand.Param param)
        {
            var result = await command.ExecuteAsync(param);
            return result;
        }
    }
}
