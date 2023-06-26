using SportAssistant.Application.UserData.Auth.Interfaces;
using SportAssistant.Domain.CustomExceptions;
using SportAssistant.Domain.DbModels.Management;
using SportAssistant.Domain.Enums;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.Common.Repositories;
using SportAssistant.Domain.Interfaces.UserData.Application;

namespace SportAssistant.Application.Management.OrganizationCommands;

/// <summary>
/// Обновление организации
/// </summary>
public class OrgUpdateCommand : ICommand<OrgUpdateCommand.Param, bool>
{
    private readonly IUserRoleCommands _userRoleCommands;
    private readonly ICrudRepo<OrganizationDb> _orgRepository;
    private readonly IUserProvider _user;

    public OrgUpdateCommand(
        IUserRoleCommands userRoleCommands,
        ICrudRepo<OrganizationDb> orgRepository,
        IUserProvider user)
    {
        _userRoleCommands = userRoleCommands;
        _orgRepository = orgRepository;
        _user = user;
    }

    public async Task<bool> ExecuteAsync(Param param)
    {
        var orgDb = await _orgRepository.FindOneAsync(t => t.Id == param.Organization.Id) ?? throw new BusinessException($"Организация не существует.");
        if (orgDb.OwnerId != _user.Id && !await _userRoleCommands.IHaveRole(UserRoles.Admin))
        {
            throw new BusinessException($"Нет прав для редактирования организации.");
        }

        if (orgDb.Name != param.Organization.Name)
        {
            var duplicateGroupsDb = await _orgRepository.FindAsync(t => t.Name == param.Organization.Name);
            if (duplicateGroupsDb.Count() > 0)
            {
                throw new BusinessException($"Организация с названием '{param.Organization.Name}' уже существует. Измените название на другое.");
            }
        }

        if (orgDb.OwnerId != param.Organization.OwnerId) // смена руководителя
        {
            await _userRoleCommands.RemoveRole(orgDb.OwnerId, UserRoles.OrgOwner);
            await _userRoleCommands.AddRole(param.Organization.OwnerId, UserRoles.OrgOwner);
        }

        orgDb.MaxCoaches = param.Organization.MaxCoaches;
        orgDb.Name = param.Organization.Name;
        orgDb.Description = param.Organization.Description;
        orgDb.OwnerId = param.Organization.OwnerId;

        _orgRepository.Update(orgDb);
        return true;
    }

    public class Param
    {
        public Organization Organization { get; set; }
    }
}
