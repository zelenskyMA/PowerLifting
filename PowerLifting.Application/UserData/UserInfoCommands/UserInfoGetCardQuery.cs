using PowerLifting.Application.Common;
using PowerLifting.Application.UserData.Auth.Interfaces;
using PowerLifting.Domain.CustomExceptions;
using PowerLifting.Domain.DbModels.UserData;
using PowerLifting.Domain.Enums;
using PowerLifting.Domain.Interfaces.Coaching.Application;
using PowerLifting.Domain.Interfaces.Common.Actions;
using PowerLifting.Domain.Interfaces.Common.Repositories;
using PowerLifting.Domain.Interfaces.UserData.Application;
using PowerLifting.Domain.Models.UserData;

namespace PowerLifting.Application.UserData.UserInfoCommands
{
    public class UserInfoGetCardQuery : ICommand<UserInfoGetCardQuery.Param, UserCard>
    {
        private readonly IUserBlockCommands _userBlockCommands;
        private readonly IUserRoleCommands _userRoleCommands;
        private readonly IProcessUserAchivements _processUserAchivements;
        private readonly IProcessTrainingGroup _processTrainingGroups;
        private readonly IProcessUserInfo _processUserInfo;
        private readonly ICrudRepo<UserDb> _userRepository;
        private readonly IUserProvider _user;

        public UserInfoGetCardQuery(
            IUserBlockCommands userBlockCommands,
            IUserRoleCommands userRoleCommands,
            IProcessUserAchivements processUserAchivements,
            IProcessTrainingGroup processTrainingGroups,
            IProcessUserInfo processUserInfo,
            ICrudRepo<UserDb> userRepository,
            IUserProvider user)
        {
            _userBlockCommands = userBlockCommands;
            _userRoleCommands = userRoleCommands;
            _processUserAchivements = processUserAchivements;
            _processTrainingGroups = processTrainingGroups;

            _processUserInfo = processUserInfo;

            _userRepository = userRepository;
            _user = user;
        }


        /// <inheritdoc />
        public async Task<UserCard> ExecuteAsync(Param param)
        {
            UserDb? userDb = null;
            if (param.UserId != 0)
            {
                userDb = (await _userRepository.FindAsync(t => t.Id == param.UserId)).FirstOrDefault();
            }

            if (!string.IsNullOrEmpty(param.Login) && userDb == null)
            {
                userDb = (await _userRepository.FindAsync(t => t.Email == param.Login)).FirstOrDefault();
            }

            if (userDb == null)
            {
                throw new BusinessException("Пользователь не найден.");
            }

            var info = await _processUserInfo.GetInfo(param.UserId);

            //доступно для просмотра админу, тренеру и себе
            if (!(await _userRoleCommands.IHaveRole(UserRoles.Admin) || info.CoachId == _user.Id || userDb.Id == _user.Id))
            {
                throw new BusinessException("Нет прав для просмотра данной информации");
            }

            var card = new UserCard()
            {
                UserId = userDb.Id,
                UserName = Naming.GetLegalFullName(info),
                Login = userDb.Email,
                BaseInfo = info,
                GroupInfo = await _processTrainingGroups.GetUserGroupAsync(param.UserId),
                Achivements = await _processUserAchivements.GetAsync(param.UserId)
            };

            if (userDb.Blocked)
            {
                card.BlockReason = await _userBlockCommands.GetCurrentBlockReason(userDb.Id);
            }

            return card;
        }

        public class Param
        {
            public int UserId { get; set; }

            public string? Login { get; set; }
        }
    }
}
