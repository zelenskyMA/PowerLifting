using AutoMapper;
using PowerLifting.Application.UserData.Auth.Interfaces;
using PowerLifting.Domain.CustomExceptions;
using PowerLifting.Domain.DbModels.UserData;
using PowerLifting.Domain.Enums;
using PowerLifting.Domain.Interfaces;
using PowerLifting.Domain.Interfaces.Common.Repositories;
using PowerLifting.Domain.Interfaces.UserData.Application;
using PowerLifting.Domain.Models;
using PowerLifting.Domain.Models.UserData;

namespace PowerLifting.Application.UserData
{
    public class UserRoleCommands : IUserRoleCommands
    {
        private readonly IDictionaryCommands _dictionaryCommands;

        private readonly ICrudRepo<UserRoleDb> _userRoleRepository;
        private readonly IUserProvider _user;
        private readonly IMapper _mapper;

        public UserRoleCommands(
            IDictionaryCommands dictionaryCommands,
            ICrudRepo<UserRoleDb> userRoleRepository,
            IUserProvider user,
            IMapper mapper)
        {
            _dictionaryCommands = dictionaryCommands;
            _userRoleRepository = userRoleRepository;
            _user = user;
            _mapper = mapper;
        }

        /// <inheritdoc />
        public async Task<List<DictionaryItem>> GetRolesList() => await _dictionaryCommands.GetItemsByTypeIdAsync(DictionaryTypes.UserRole);

        /// <inheritdoc />
        public async Task<List<UserRole>> GetUserRoles(int userId) => (await _userRoleRepository.FindAsync(t => t.UserId == userId))
            .Select(t => _mapper.Map<UserRole>(t)).ToList();

        /// <inheritdoc />
        public async Task<bool> IHaveRole(UserRoles role) =>
            (await _userRoleRepository.FindAsync(t => t.UserId == _user.Id && t.RoleId == (int)role)).Any();

        /// <inheritdoc />
        public async Task AddRole(int userId, UserRoles role)
        {
            if (!await IHaveRole(UserRoles.Admin))
            {
                throw new RoleException();
            }

            if ((await _userRoleRepository.FindAsync(t => t.UserId == userId && t.RoleId == (int)role)).Any())
            {
                return;
            }

            await _userRoleRepository.CreateAsync(new UserRoleDb() { UserId = userId, RoleId = (int)role });
        }

        /// <inheritdoc />
        public async Task RemoveRole(int userId, UserRoles role)
        {
            if (!await IHaveRole(UserRoles.Admin))
            {
                throw new RoleException();
            }

            if (userId == _user.Id)
            {
                throw new BusinessException("Нельзя забрать роль у самого себя");
            }

            var roleDb = await _userRoleRepository.FindAsync(t => t.UserId == userId && t.RoleId == (int)role);
            if (roleDb.Count != 0)
            {
                await _userRoleRepository.DeleteAsync(roleDb.First());
            }
        }
    }
}
