using AutoMapper;
using PowerLifting.Application.UserData.Auth.Interfaces;
using PowerLifting.Domain.CustomExceptions;
using PowerLifting.Domain.DbModels.Coaching;
using PowerLifting.Domain.DbModels.UserData;
using PowerLifting.Domain.Interfaces.Coaching.Application;
using PowerLifting.Domain.Interfaces.Coaching.Repositories;
using PowerLifting.Domain.Interfaces.Common.Repositories;
using PowerLifting.Domain.Models.Coaching;

namespace PowerLifting.Application.Coaching
{
    public class UserTrainingGroupCommands : IUserTrainingGroupCommands
    {
        private readonly ITrainingRequestCommands _trainingRequestCommands;
        private readonly IUserTrainingGroupRepository _userTrainingGroupRepository;
        private readonly ICrudRepo<TrainingGroupDb> _trainingGroupRepository;
        private readonly ICrudRepo<UserInfoDb> _userInfoRepository;
        private readonly IUserProvider _user;
        private readonly IMapper _mapper;

        public UserTrainingGroupCommands(
         ITrainingRequestCommands trainingRequestCommands,
         IUserTrainingGroupRepository userTrainingGroupRepository,
         ICrudRepo<TrainingGroupDb> trainingGroupRepository,
         ICrudRepo<UserInfoDb> userInfoRepository,
         IUserProvider user,
         IMapper mapper)
        {
            _trainingRequestCommands = trainingRequestCommands;

            _trainingGroupRepository = trainingGroupRepository;
            _userTrainingGroupRepository = userTrainingGroupRepository;
            _userInfoRepository = userInfoRepository;
            _user = user;
            _mapper = mapper;
        }

        /// <inheritdoc />
        public async Task UpdateUserGroup(UserTrainingGroup targetGroup)
        {
            (TrainingGroupDb group, UserInfoDb userInfo) = await CheckAssignmentAvailable(targetGroup);

            var userRequestDb = await _trainingRequestCommands.GetUserRequestAsync(targetGroup.UserId);
            if (userRequestDb != null)
            {
                await AddNewUserToGroup(group, userInfo, userRequestDb);
                return;
            }

            await ChangeUserGroup(group, userInfo);
        }

        /// <inheritdoc />
        public async Task RemoveUserFromGroup(UserTrainingGroup targetGroup)
        {
            await CheckAssignmentAvailable(targetGroup);

            var userGroupsDb = await _userTrainingGroupRepository.FindAsync(t => t.UserId == targetGroup.UserId && t.GroupId == targetGroup.GroupId);
            if (userGroupsDb.Any())
            {
                await _userTrainingGroupRepository.DeleteAsync(userGroupsDb.First());
            }
        }


        /// <summary> Тренер перемещает спортсмена между своими группами. </summary>
        private async Task ChangeUserGroup(TrainingGroupDb group, UserInfoDb userInfo)
        {
            if (userInfo.CoachId != _user.Id)
            {
                throw new BusinessException("Спортсмен тренируется у другого тренера.");
            }

            var userGroupDb = (await _userTrainingGroupRepository.FindAsync(t => t.UserId == userInfo.UserId)).FirstOrDefault();
            if (userGroupDb == null || userGroupDb.GroupId == group.Id) //нет реального перемещения.
            {
                return;
            }

            userGroupDb.GroupId = group.Id;
            await _userTrainingGroupRepository.UpdateAsync(userGroupDb);
        }


        /// <summary> По заявке определяем спортсмена к тренеру в указанную группу. Заявку удаляем. </summary>
        private async Task AddNewUserToGroup(TrainingGroupDb group, UserInfoDb userInfo, TrainingRequest request)
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

                await _userTrainingGroupRepository.CreateAsync(new UserTrainingGroupDb()
                {
                    UserId = userInfo.UserId,
                    GroupId = group.Id
                });

                userInfo.CoachId = _user.Id;
                await _userInfoRepository.UpdateAsync(userInfo);
            }
            finally
            {
                await _trainingRequestCommands.RemoveRequestAsync(userInfo.UserId);
            }
        }


        /// <summary> Проверяем что сущности существуют и группа принадлежит тренеру, выполняющему операцию </summary>
        private async Task<(TrainingGroupDb group, UserInfoDb userInfo)> CheckAssignmentAvailable(UserTrainingGroup targetGroup)
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
}
