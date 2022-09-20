using AutoMapper;
using PowerLifting.Application.UserData.Auth.Interfaces;
using PowerLifting.Domain.CustomExceptions;
using PowerLifting.Domain.DbModels.UserData;
using PowerLifting.Domain.Enums;
using PowerLifting.Domain.Interfaces.Common.Repositories;
using PowerLifting.Domain.Interfaces.UserData.Application;
using PowerLifting.Domain.Models.UserData;

namespace PowerLifting.Application.UserData
{
    public class UserInfoCommands : IUserInfoCommands
    {
        private readonly IUserBlockCommands _userBlockCommands;
        private readonly IUserRoleCommands _userRoleCommands;

        private readonly ICrudRepo<UserInfoDb> _userInfoRepository;
        private readonly ICrudRepo<UserDb> _userRepository;
        private readonly IUserProvider _user;
        private readonly IMapper _mapper;

        public UserInfoCommands(
            IUserBlockCommands userBlockCommands,
            IUserRoleCommands userRoleCommands,
            ICrudRepo<UserInfoDb> userInfoRepository,
            ICrudRepo<UserDb> userRepository,
            IUserProvider user,
            IMapper mapper)
        {
            _userBlockCommands = userBlockCommands;
            _userRoleCommands = userRoleCommands;
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

        public async Task<UserCard> GetUserCardAsync(int userId, string login)
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
                Login = userDb.Email,
                BaseInfo = info,
            };

            if (info.CoachId > 0)
            {
                card.CoachLegalName = (await GetInfo(info.CoachId.Value)).LegalName;
            }

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

            string patronimic = string.IsNullOrEmpty(info.Patronimic) ? string.Empty : $" {info.Patronimic.ToUpper().First()}.";
            string firstName = string.IsNullOrEmpty(info.FirstName) ? string.Empty : $" {info.FirstName?.ToUpper()?.First()}.";
            info.LegalName = string.IsNullOrEmpty(info.Surname) ? "Кабинет" : $"{info.Surname}{firstName}{patronimic}";

            info.RolesInfo = await _userRoleCommands.GetUserRoles(userId);

            return info;
        }
    }
}
