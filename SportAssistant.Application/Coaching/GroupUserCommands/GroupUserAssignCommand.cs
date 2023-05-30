using SportAssistant.Application.UserData.Auth.Interfaces;
using SportAssistant.Domain.CustomExceptions;
using SportAssistant.Domain.DbModels.Coaching;
using SportAssistant.Domain.DbModels.UserData;
using SportAssistant.Domain.Interfaces.Coaching.Application;
using SportAssistant.Domain.Interfaces.Coaching.Repositories;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.Common.Repositories;
using SportAssistant.Domain.Interfaces.TrainingPlan.Application;
using SportAssistant.Domain.Models.Coaching;
using SportAssistant.Infrastructure.DataContext;

namespace SportAssistant.Application.Coaching.TrainingGroupUserCommands;

/// <summary>
/// Назначение/изменение тренировочной группы, к которой прикреплен спортсмен
/// </summary>
public class GroupUserAssignCommand : ICommand<GroupUserAssignCommand.Param, bool>
{
    private readonly IContextProvider _provider;
    private readonly IProcessRequest _processTrainingRequest;
    private readonly IProcessGroupUser _processTrainingGroupUser;
    private readonly ITrainingGroupUserRepository _trainingGroupUserRepository;
    private readonly ICrudRepo<UserInfoDb> _userInfoRepository;
    private readonly IUserProvider _user;

    public GroupUserAssignCommand(
        IContextProvider provider,
        IProcessGroupUser processTrainingGroupUser,
        IProcessRequest processTrainingRequest,
        ITrainingGroupUserRepository trainingGroupUserRepository,
        ICrudRepo<UserInfoDb> userInfoRepository,
        IUserProvider user)
    {
        _provider = provider;
        _processTrainingGroupUser = processTrainingGroupUser;
        _processTrainingRequest = processTrainingRequest;
        _trainingGroupUserRepository = trainingGroupUserRepository;
        _userInfoRepository = userInfoRepository;
        _user = user;
    }

    public async Task<bool> ExecuteAsync(Param param)
    {
        (TrainingGroupDb group, UserInfoDb userInfo) = await _processTrainingGroupUser.CheckAssignmentAvailable(param.UserGroup);

        var userRequestDb = await _processTrainingRequest.GetByUserAsync(param.UserGroup.UserId);
        if (userRequestDb.Id == 0)
        {
            if (userInfo.CoachId != _user.Id)
            {
                throw new BusinessException("Нельзя принять спортсмена в группу без его заявки");
            }

            await TransferUserBetweenGroupsAsync(group, userInfo);
            return true;
        }

        await AssignUserByRequestAsync(group, userInfo, userRequestDb);
        return true;
    }

    /// <summary> По заявке определяем спортсмена к тренеру в указанную группу. Заявку удаляем. </summary>
    private async Task AssignUserByRequestAsync(TrainingGroupDb group, UserInfoDb userInfo, TrainingRequest request)
    {
        if (request.CoachId != _user.Id)
        {
            throw new BusinessException("Заявка спортсмена направлена другому тренеру");
        }

        try
        {
            await _trainingGroupUserRepository.CreateAsync(new TrainingGroupUserDb()
            {
                UserId = userInfo.UserId,
                GroupId = group.Id
            });

            userInfo.CoachId = _user.Id;
            _userInfoRepository.Update(userInfo);
        }
        finally
        {
            await _processTrainingRequest.RemoveAsync(userInfo.UserId);
        }
    }


    private async Task TransferUserBetweenGroupsAsync(TrainingGroupDb group, UserInfoDb userInfo)
    {
        var userGroupDb = (await _trainingGroupUserRepository.FindAsync(t => t.UserId == userInfo.UserId)).FirstOrDefault();
        if (userGroupDb == null || userGroupDb.GroupId == group.Id) //нет реального перемещения.
        {
            return;
        }

        _trainingGroupUserRepository.Delete(userGroupDb); // нельзя обновить часть ключа, потому через удаление
        await _provider.AcceptChangesAsync();

        await _trainingGroupUserRepository.CreateAsync(new TrainingGroupUserDb()
        {
            UserId = userInfo.UserId,
            GroupId = group.Id
        });
    }

    public class Param
    {
        public TrainingGroupUser UserGroup { get; set; }
    }
}
