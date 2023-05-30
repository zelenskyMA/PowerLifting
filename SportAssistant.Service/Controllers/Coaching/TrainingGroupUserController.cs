using Microsoft.AspNetCore.Mvc;
using SportAssistant.Application.Coaching.TrainingGroupUserCommands;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Models.Coaching;

namespace SportAssistant.Service.Controllers.Coaching;

[Route("groupUser")]
public class TrainingGroupUserController : BaseController
{
    [HttpPost]
    [Route("assign")]
    public async Task<bool> AssignUserGroupAsync([FromServices] ICommand<GroupUserAssignCommand.Param, bool> command, TrainingGroupUser targetGroup)
    {
        var result = await command.ExecuteAsync(new GroupUserAssignCommand.Param() { UserGroup = targetGroup });
        return result;
    }

    [HttpPost]
    [Route("reject")]
    public async Task<bool> RejectCoach([FromServices] ICommand<GroupUserRejectCommand.Param, bool> command)
    {
        var result = await command.ExecuteAsync(new GroupUserRejectCommand.Param() { });
        return result;
    }

    [HttpPost]
    [Route("remove")]
    public async Task<bool> RemoveUserFromGroup([FromServices] ICommand<GroupUserRemoveCommand.Param, bool> command, TrainingGroupUser targetGroup)
    {
        var result = await command.ExecuteAsync(new GroupUserRemoveCommand.Param() { UserGroup = targetGroup });
        return result;
    }
}
