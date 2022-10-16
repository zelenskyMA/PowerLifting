using PowerLifting.Application.UserData.Auth.Interfaces;
using PowerLifting.Domain.CustomExceptions;
using PowerLifting.Domain.DbModels.Coaching;
using PowerLifting.Domain.DbModels.UserData;
using PowerLifting.Domain.Interfaces.Coaching.Application;
using PowerLifting.Domain.Interfaces.Coaching.Repositories;
using PowerLifting.Domain.Interfaces.Common.Actions;
using PowerLifting.Domain.Interfaces.Common.Repositories;
using PowerLifting.Domain.Interfaces.TrainingPlan.Application;
using PowerLifting.Domain.Models.Coaching;
using PowerLifting.Infrastructure.Setup;

namespace PowerLifting.Application.Coaching.TrainingGroupUserCommands
{
    /// <summary>
    /// Изменение тренировочной группы, к которой прикреплен спортсмен
    /// </summary>
    public class TrainingGroupUserUpdateCommand : ICommand<TrainingGroupUserUpdateCommand.Param, bool>
    {
        private readonly IContextProvider _provider;
        private readonly IProcessTrainingRequest _processTrainingRequest;
        private readonly IProcessTrainingGroupUser _processTrainingGroupUser;
        private readonly ITrainingGroupUserRepository _trainingGroupUserRepository;
        private readonly ICrudRepo<UserInfoDb> _userInfoRepository;
        private readonly IUserProvider _user;

        public TrainingGroupUserUpdateCommand(
            IContextProvider provider,
            IProcessTrainingGroupUser processTrainingGroupUser,
            IProcessTrainingRequest processTrainingRequest,
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
                await UpdateUserToGroupAsync(group, userInfo);
                return true;
            }

            await AddNewUserToGroupAsync(group, userInfo, userRequestDb);
            return true;
        }

        /// <summary> По заявке определяем спортсмена к тренеру в указанную группу. Заявку удаляем. </summary>
        private async Task AddNewUserToGroupAsync(TrainingGroupDb group, UserInfoDb userInfo, TrainingRequest request)
        {
            if (request.CoachId != _user.Id)
            {
                throw new BusinessException("Заявка спортсмена направлена другому тренеру");
            }

            try
            {
                if (userInfo.CoachId != null)
                {
                    throw new BusinessException("Спортсмен уже тренируется у другого тренера. Ошибочная заявка.");
                }

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


        private async Task UpdateUserToGroupAsync(TrainingGroupDb group, UserInfoDb userInfo)
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
}
