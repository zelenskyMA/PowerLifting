using Microsoft.AspNetCore.Mvc;
using PowerLifting.Application.UserData.UserAchivementCommands;
using PowerLifting.Application.UserData.UserCommands1;
using PowerLifting.Domain.Interfaces.Common.Operations;
using PowerLifting.Domain.Models.UserData;

namespace PowerLifting.Service.Controllers.UserData
{
    [Route("userAchivement")]
    public class UserAchivementController : BaseController
    {
        [HttpGet]
        [Route("get")]
        public async Task<List<UserAchivement>> GetAsync([FromServices] ICommand<UserAchivementGetQuery.Param, List<UserAchivement>> command)
        {
            var result = await command.ExecuteAsync(new UserAchivementGetQuery.Param());
            return result;
        }

        [HttpGet]
        [Route("getByExercise")]
        public async Task<UserAchivement> GetByExerciseAsync([FromServices] ICommand<UserAchivementsGetByExerciseQuery.Param, UserAchivement> command, int userId, int exerciseTypeId)
        {
            var result = await command.ExecuteAsync(new UserAchivementsGetByExerciseQuery.Param() { UserId = userId, ExerciseTypeId = exerciseTypeId });
            return result;
        }

        [HttpPost]
        [Route("create")]
        public async Task<bool> CreateAsync([FromServices] ICommand<UserAchivementCreateCommand.Param, bool> command, List<UserAchivement> achivements)
        {
            var result = await command.ExecuteAsync(new UserAchivementCreateCommand.Param() { Achivements = achivements });
            return result;
        }
    }
}
