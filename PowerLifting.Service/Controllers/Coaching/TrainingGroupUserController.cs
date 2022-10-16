using Microsoft.AspNetCore.Mvc;
using PowerLifting.Application.Coaching.TrainingGroupUserCommands;
using PowerLifting.Domain.Interfaces.Common.Actions;
using PowerLifting.Domain.Models.Coaching;

namespace PowerLifting.Service.Controllers.Coaching
{
    [Route("groupUser")]
    public class TrainingGroupUserController : BaseController
    {
        [HttpPost]
        [Route("update")]
        public async Task<bool> UpdateUserGroupAsync([FromServices] ICommand<TrainingGroupUserUpdateCommand.Param, bool> command, TrainingGroupUser targetGroup)
        {
            var result = await command.ExecuteAsync(new TrainingGroupUserUpdateCommand.Param() { UserGroup = targetGroup });
            return result;
        }

        [HttpPost]
        [Route("reject")]
        public async Task<bool> RejectCoach([FromServices] ICommand<TrainingGroupUserRejectCommand.Param, bool> command)
        {
            var result = await command.ExecuteAsync(new TrainingGroupUserRejectCommand.Param() { });
            return result;
        }

        [HttpPost]
        [Route("remove")]
        public async Task<bool> RemoveUserFromGroup([FromServices] ICommand<TrainingGroupUserRemoveCommand.Param, bool> command, TrainingGroupUser targetGroup)
        {
            var result = await command.ExecuteAsync(new TrainingGroupUserRemoveCommand.Param() { UserGroup = targetGroup });
            return result;
        }
    }
}
