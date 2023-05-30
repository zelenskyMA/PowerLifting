using SportAssistant.Application.UserData.Auth.Interfaces;
using SportAssistant.Domain.CustomExceptions;
using SportAssistant.Domain.DbModels.Coaching;
using SportAssistant.Domain.DbModels.UserData;
using SportAssistant.Domain.Interfaces.Common.Repositories;
using SportAssistant.Domain.Interfaces.TrainingPlan.Application;
using SportAssistant.Domain.Models.Coaching;

namespace SportAssistant.Application.Coaching.TrainingGroupUserCommands;

public class ProcessGroupUser : IProcessGroupUser
{
    private readonly ICrudRepo<TrainingGroupDb> _trainingGroupRepository;
    private readonly ICrudRepo<UserInfoDb> _userInfoRepository;
    private readonly IUserProvider _user;

    public ProcessGroupUser(
        ICrudRepo<TrainingGroupDb> trainingGroupRepository,
        ICrudRepo<UserInfoDb> userInfoRepository,
        IUserProvider user)
    {
        _trainingGroupRepository = trainingGroupRepository;
        _userInfoRepository = userInfoRepository;
        _user = user;
    }

    /// <summary> Проверяем что сущности существуют и группа принадлежит тренеру, выполняющему операцию </summary>
    public async Task<(TrainingGroupDb group, UserInfoDb userInfo)> CheckAssignmentAvailable(TrainingGroupUser targetGroup)
    {
        var groupsDb = await _trainingGroupRepository.FindAsync(t => t.Id == targetGroup.GroupId);
        if (!groupsDb.Any())
        {
            throw new BusinessException("Группа не найдена");
        }

        var usersInfoDb = await _userInfoRepository.FindAsync(t => t.UserId == targetGroup.UserId);
        if (!usersInfoDb.Any())
        {
            throw new BusinessException("Пользователь не найден");
        }

        var groupDb = groupsDb.First();
        var userInfoDb = usersInfoDb.First();

        if (groupDb.CoachId != _user.Id)
        {
            throw new BusinessException("Вы не являетесь тренером указанной группы");
        }

        return (groupDb, userInfoDb);
    }
}
