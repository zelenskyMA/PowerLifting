using AutoMapper;
using SportAssistant.Application.UserData.Auth.Interfaces;
using SportAssistant.Domain.CustomExceptions;
using SportAssistant.Domain.DbModels.UserData;
using SportAssistant.Domain.Enums;
using SportAssistant.Domain.Interfaces;
using SportAssistant.Domain.Interfaces.Common.Repositories;
using SportAssistant.Domain.Interfaces.UserData.Application;
using SportAssistant.Domain.Models;
using SportAssistant.Domain.Models.UserData;

namespace SportAssistant.Application.UserData;

public class UserRoleCommands : IUserRoleCommands
{
    private readonly IProcessDictionary _processDictionary;

    private readonly ICrudRepo<UserRoleDb> _userRoleRepository;
    private readonly IUserProvider _user;
    private readonly IMapper _mapper;

    public UserRoleCommands(
        IProcessDictionary dictionaryCommands,
        ICrudRepo<UserRoleDb> userRoleRepository,
        IUserProvider user,
        IMapper mapper)
    {
        _processDictionary = dictionaryCommands;
        _userRoleRepository = userRoleRepository;
        _user = user;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<List<DictionaryItem>> GetRolesList() => await _processDictionary.GetItemsByTypeIdAsync(DictionaryTypes.UserRole);

    /// <inheritdoc />
    public async Task<RolesInfo> GetUserRoles(int userId)
    {
        var roles = (await _userRoleRepository.FindAsync(t => t.UserId == userId)).Select(_mapper.Map<UserRole>).ToList();
        var roleInfo = new RolesInfo();
        foreach (var item in roles)
        {
            switch (item.RoleId)
            {
                case (int)UserRoles.Admin: roleInfo.IsAdmin = true; break;
                case (int)UserRoles.Coach: roleInfo.IsCoach = true; break;
                case (int)UserRoles.Manager: roleInfo.IsManager = true; break;
                case (int)UserRoles.OrgOwner: roleInfo.IsOrgOwner = true; break;
            }
        }

        return roleInfo;
    }

    /// <inheritdoc />
    public async Task<bool> IHaveRole(UserRoles role) =>
        (await _userRoleRepository.FindAsync(t => t.UserId == _user.Id && t.RoleId == (int)role)).Any();

    /// <inheritdoc />
    public async Task<bool> IHaveAnyRoles(IEnumerable<UserRoles> roles)
    {
        List<int> roleIds = roles.Cast<int>().ToList();
        return (await _userRoleRepository.FindAsync(t => t.UserId == _user.Id && roleIds.Contains(t.RoleId))).Any();
    }

    /// <inheritdoc />
    public async Task AddRole(int userId, UserRoles role)
    {
        await CheckRightsAsync(role);

        if ((await _userRoleRepository.FindAsync(t => t.UserId == userId && t.RoleId == (int)role)).Any())
        {
            return;
        }

        await _userRoleRepository.CreateAsync(new UserRoleDb() { UserId = userId, RoleId = (int)role });
    }

    /// <inheritdoc />
    public async Task RemoveRole(int userId, UserRoles role)
    {
        await CheckRightsAsync(role);

        if (userId == _user.Id && role == UserRoles.Admin)
        {
            throw new BusinessException("Нельзя забрать роль администратора у самого себя");
        }

        var roleDb = await _userRoleRepository.FindAsync(t => t.UserId == userId && t.RoleId == (int)role);
        if (roleDb.Count != 0)
        {
            _userRoleRepository.Delete(roleDb.First());
        }
    }

    private async Task CheckRightsAsync(UserRoles role)
    {
        if (await IHaveRole(UserRoles.Admin))
        {
            return; // админ может все
        }

        if (await IHaveRole(UserRoles.OrgOwner) && new[] { UserRoles.OrgOwner, UserRoles.Manager }.Contains(role))
        {
            return; // владелец может выбирать другого владельца и менеджеров
        }

        if (await IHaveRole(UserRoles.Manager) && new[] { UserRoles.Coach }.Contains(role))
        {
            return; // менеджер может назначать тренеров
        }

        throw new RoleException();
    }
}
