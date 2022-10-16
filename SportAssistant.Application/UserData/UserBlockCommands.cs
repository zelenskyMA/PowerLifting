using AutoMapper;
using SportAssistant.Application.UserData.Auth.Interfaces;
using SportAssistant.Domain.CustomExceptions;
using SportAssistant.Domain.DbModels.UserData;
using SportAssistant.Domain.Enums;
using SportAssistant.Domain.Interfaces.Common.Repositories;
using SportAssistant.Domain.Interfaces.UserData.Application;
using SportAssistant.Domain.Models.UserData;
using SportAssistant.Infrastructure.Repositories.UserData;

namespace SportAssistant.Application.UserData
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
            var blocks = await _userBlockHistoryRepository.FindAsync(t => t.UserId == userId);
            if (blocks.Count == 0)
            {
                return null;
            }

            return _mapper.Map<UserBlockHistory>(blocks.OrderByDescending(t => t.CreationDate).First());
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

            if (userDb.Id == _user.Id)
            {
                throw new BusinessException("Нельзя заблокировать самого себя");
            }

            userDb.Blocked = true;
            _userRepository.Update(userDb);

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
            _userRepository.Update(userDb);
        }

    }
}
