using AutoMapper;
using PowerLifting.Application.UserData.Auth.Interfaces;
using PowerLifting.Domain.CustomExceptions;
using PowerLifting.Domain.DbModels.UserData;
using PowerLifting.Domain.Enums;
using PowerLifting.Domain.Interfaces.Common.Repositories;
using PowerLifting.Domain.Interfaces.UserData.Application;
using PowerLifting.Domain.Models.UserData;
using PowerLifting.Infrastructure.Repositories.UserData;

namespace PowerLifting.Application.UserData
{
    public class UserBlockCommands : IUserBlockCommands
    {
        private readonly IUserRoleCommands _userRoleCommands;

        private readonly ICrudRepo<UserBlockHistoryDb> _userBlockHistoryRepository;
        private readonly ICrudRepo<UserDb> _userRepository;
        private readonly IUserProvider _user;
        private readonly IMapper _mapper;

        public UserBlockCommands(
            IUserRoleCommands userRoleCommands,
            ICrudRepo<UserBlockHistoryDb> userBlockHistoryRepository,
            ICrudRepo<UserDb> userRepository,
            IUserProvider user,
            IMapper mapper)
        {
            _userRoleCommands = userRoleCommands;
            _userBlockHistoryRepository = userBlockHistoryRepository;
            _userRepository = userRepository;
            _user = user;
            _mapper = mapper;
        }

        public async Task<UserBlockHistory> GetCurrentBlockReason(int userId)
        {
            var bloscks = await _userBlockHistoryRepository.FindAsync(t => t.UserId == userId);
            if (bloscks.Count == 0)
            {
                return null;
            }

            return _mapper.Map<UserBlockHistory>(bloscks.OrderByDescending(t => t.CreationDate).First());
        }

        public async Task BlockUser(int userId, string reason)
        {
            if (string.IsNullOrEmpty(reason))
            {
                throw new BusinessException($"Причина блокировки пользователя обязательна");
            }

            await _userRoleCommands.IHaveRole(UserRoles.Admin);

            var users = await _userRepository.FindAsync(t => t.Id == userId);
            if (users.Count == 0)
            {
                throw new BusinessException($"Пользователья с Ид {userId} не найден.");
            }

            var userDb = users.First();
            if (userDb.Blocked)
            {
                throw new BusinessException($"Пользователья с Ид {userId} уже заблокирован.");
            }

            userDb.Blocked = true;
            await _userRepository.UpdateAsync(userDb);

            await _userBlockHistoryRepository.CreateAsync(new UserBlockHistoryDb()
            {
                UserId = userId,
                Reason = reason,
                BlockerId = _user.Id,
                CreationDate = DateTime.Now
            });
        }

        public async Task UnblockUser(int userId)
        {
            await _userRoleCommands.IHaveRole(UserRoles.Admin);

            var users = await _userRepository.FindAsync(t => t.Id == userId);
            if (users.Count == 0)
            {
                throw new BusinessException($"Пользователья с Ид {userId} не найден.");
            }

            var userDb = users.First();
            if (!userDb.Blocked)
            {
                throw new BusinessException($"Пользователья с Ид {userId} не заблокирован.");
            }

            userDb.Blocked = false;
            await _userRepository.UpdateAsync(userDb);
        }

    }
}
