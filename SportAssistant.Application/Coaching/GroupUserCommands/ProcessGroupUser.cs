using AutoMapper;
using NPOI.Util;
using SportAssistant.Application.Common.Actions;
using SportAssistant.Application.UserData.Auth.Interfaces;
using SportAssistant.Domain.CustomExceptions;
using SportAssistant.Domain.DbModels.Coaching;
using SportAssistant.Domain.DbModels.UserData;
using SportAssistant.Domain.Interfaces.Coaching.Repositories;
using SportAssistant.Domain.Interfaces.Common.Repositories;
using SportAssistant.Domain.Interfaces.TrainingPlan.Application;
using SportAssistant.Domain.Models.Coaching;
using SportAssistant.Domain.Models.UserData;

namespace SportAssistant.Application.Coaching.TrainingGroupUserCommands;

public class ProcessGroupUser : IProcessGroupUser
{
    private readonly ICrudRepo<TrainingGroupDb> _trainingGroupRepository;
    private readonly ITrainingGroupUserRepository _trainingGroupUserRepository;
    private readonly ICrudRepo<UserInfoDb> _userInfoRepository;
    private readonly IMapper _mapper;
    private readonly IUserProvider _user;

    public ProcessGroupUser(
        ICrudRepo<TrainingGroupDb> trainingGroupRepository,
        ITrainingGroupUserRepository trainingGroupUserRepository,
        ICrudRepo<UserInfoDb> userInfoRepository,
        IMapper mapper,
        IUserProvider user)
    {
        _trainingGroupRepository = trainingGroupRepository;
        _trainingGroupUserRepository = trainingGroupUserRepository;
        _userInfoRepository = userInfoRepository;
        _mapper = mapper;
        _user = user;
    }

    /// <inheritdoc />
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

    /// <inheritdoc />
    public async Task<List<UserInfo>> GetCoachUsersList(int coachId)
    {
        var usersInfoDb = await _trainingGroupUserRepository.GetCoachUsersFullListAsync(coachId);

        var usersInfo = usersInfoDb.Select(_mapper.Map<UserInfo>).ToList();
        foreach (var info in usersInfo)
        {
            info.LegalName = UserNaming.GetLegalFullName(info, "Имя не задано");
        }

        return usersInfo;
    }
}
