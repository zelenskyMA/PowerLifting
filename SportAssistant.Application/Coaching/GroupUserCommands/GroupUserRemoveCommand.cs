using SportAssistant.Domain.DbModels.Coaching;
using SportAssistant.Domain.DbModels.UserData;
using SportAssistant.Domain.Interfaces.Coaching.Repositories;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.Common.Repositories;
using SportAssistant.Domain.Interfaces.TrainingPlan.Application;
using SportAssistant.Domain.Models.Coaching;

namespace SportAssistant.Application.Coaching.TrainingGroupUserCommands;

/// <summary>
/// Отказ тренера от спортсмена.Исключение его из тренировочной группы.
/// </summary>
public class GroupUserRemoveCommand : ICommand<GroupUserRemoveCommand.Param, bool>
{
    private readonly IProcessGroupUser _processTrainingGroupUser;
    private readonly ITrainingGroupUserRepository _trainingGroupUserRepository;
    private readonly ICrudRepo<UserInfoDb> _userInfoRepository;

    public GroupUserRemoveCommand(
        IProcessGroupUser processTrainingGroupUser,
        ITrainingGroupUserRepository trainingGroupUserRepository,
        ICrudRepo<UserInfoDb> userInfoRepository)
    {
        _processTrainingGroupUser = processTrainingGroupUser;
        _trainingGroupUserRepository = trainingGroupUserRepository;
        _userInfoRepository = userInfoRepository;
    }

    public async Task<bool> ExecuteAsync(Param param)
    {
        (TrainingGroupDb group, UserInfoDb userInfo) = await _processTrainingGroupUser.CheckAssignmentAvailable(param.UserGroup);

        var userGroupsDb = await _trainingGroupUserRepository.FindAsync(t => t.UserId == param.UserGroup.UserId && t.GroupId == param.UserGroup.GroupId);
        if (userGroupsDb.Any())
        {
            _trainingGroupUserRepository.Delete(userGroupsDb.First());

            userInfo.CoachId = null;
            _userInfoRepository.Update(userInfo);
        }

        return true;
    }

    public class Param
    {
        public TrainingGroupUser UserGroup { get; set; }
    }
}
