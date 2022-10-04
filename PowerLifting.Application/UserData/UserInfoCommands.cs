using AutoMapper;
using PowerLifting.Application.Coaching;
using PowerLifting.Application.Common;
using PowerLifting.Application.UserData.Auth.Interfaces;
using PowerLifting.Domain.CustomExceptions;
using PowerLifting.Domain.DbModels.UserData;
using PowerLifting.Domain.Enums;
using PowerLifting.Domain.Interfaces.Coaching.Application;
using PowerLifting.Domain.Interfaces.Common.Repositories;
using PowerLifting.Domain.Interfaces.UserData.Application;
using PowerLifting.Domain.Models.UserData;

namespace PowerLifting.Application.UserData
{
    public class UserInfoCommands : IUserInfoCommands
    {
        private readonly IUserBlockCommands _userBlockCommands;
        private readonly IUserRoleCommands _userRoleCommands;
        private readonly IUserAchivementCommands _userAchivementCommands;
        private readonly ITrainingGroupCommands _trainingGroupCommands;

        private readonly ICrudRepo<UserInfoDb> _userInfoRepository;
        private readonly ICrudRepo<UserDb> _userRepository;
        private readonly IUserProvider _user;
        private readonly IMapper _mapper;

        public UserInfoCommands(
            IUserBlockCommands userBlockCommands,
            IUserRoleCommands userRoleCommands,
            IUserAchivementCommands userAchivementCommands,
            ITrainingGroupCommands trainingGroupCommands,
            ICrudRepo<UserInfoDb> userInfoRepository,
            ICrudRepo<UserDb> userRepository,
            IUserProvider user,
            IMapper mapper)
        {
            _userBlockCommands = userBlockCommands;
            _userRoleCommands = userRoleCommands;
            _userAchivementCommands = userAchivementCommands;
            _trainingGroupCommands = trainingGroupCommands;

            _userInfoRepository = userInfoRepository;
            _userRepository = userRepository;
            _user = user;
            _mapper = mapper;
        }

        /// <inheritdoc />
        public async Task<UserInfo> GetAsync()
        {
            var info = await GetInfo(_user.Id);
            return info;
        }

        /// <inheritdoc />
        public async Task UpdateAsync(UserInfo userInfo)
        {
            var userInfoDb = _mapper.Map<UserInfoDb>(userInfo);
            userInfoDb.UserId = _user.Id;

            await _userInfoRepository.UpdateAsync(userInfoDb);
        }

        public async Task<UserCard> GetUserCardAsync(int userId, string? login)
        {
            UserDb? userDb = null;
            if (userId != 0)
            {
                userDb = (await _userRepository.FindAsync(t => t.Id == userId)).FirstOrDefault();
            }

            if (!string.IsNullOrEmpty(login) && userDb == null)
            {
                userDb = (await _userRepository.FindAsync(t => t.Email == login)).FirstOrDefault();
            }

            if (userDb == null)
            {
                throw new BusinessException("Пользователь не найден.");
            }

            var info = await GetInfo(userId);

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
                GroupInfo = await _trainingGroupCommands.GetUserGroupAsync(userId),
                Achivements = await _userAchivementCommands.GetAsync(userId)
            };

            if (userDb.Blocked)
            {
                card.BlockReason = await _userBlockCommands.GetCurrentBlockReason(userDb.Id);
            }

            return card;
        }

        private async Task<UserInfo> GetInfo(int userId)
        {
            var infoDb = (await _userInfoRepository.FindAsync(t => t.UserId == userId)).FirstOrDefault();
            if (infoDb == null)
            {
                return new UserInfo();
            }

            var info = _mapper.Map<UserInfo>(infoDb);
            info.LegalName = Naming.GetLegalShortName(info.FirstName, info.Surname, info.Patronimic, "Кабинет");
            info.RolesInfo = await _userRoleCommands.GetUserRoles(userId);

            if (info.CoachId > 0)
            {
                info.CoachLegalName = (await GetInfo(info.CoachId.Value)).LegalName;
            }

            return info;
        }
    }
}
