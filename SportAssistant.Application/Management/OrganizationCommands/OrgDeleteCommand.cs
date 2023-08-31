using SportAssistant.Application.UserData.Auth.Interfaces;
using SportAssistant.Domain.CustomExceptions;
using SportAssistant.Domain.DbModels.Management;
using SportAssistant.Domain.Enums;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.Common.Repositories;
using SportAssistant.Domain.Interfaces.UserData.Application;

namespace SportAssistant.Application.Management.OrganizationCommands;

/// <summary>
/// Удаление организации
/// </summary>
public class OrgDeleteCommand : ICommand<OrgDeleteCommand.Param, bool>
{
    private readonly IUserRoleCommands _userRoleCommands;
    private readonly ICrudRepo<OrganizationDb> _orgRepository;
    private readonly IUserProvider _user;

    public OrgDeleteCommand(
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
        var orgDb = await _orgRepository.FindOneAsync(t => t.Id == param.Id) ?? throw new BusinessException($"Компания с Ид '{param.Id}' не найдена");        
        if (orgDb.OwnerId != _user.Id && !await _userRoleCommands.IHaveRole(UserRoles.Admin))
        {
            throw new BusinessException($"Нет прав для редактирования организации.");
        }

        await _userRoleCommands.RemoveRole(orgDb.OwnerId, UserRoles.OrgOwner);
        // TODO: удаляем менеджеров организации и всех тренеров, которых они назначили

        _orgRepository.Delete(orgDb);
        return true;
    }

    public class Param
    {
        public int Id { get; set; }
    }
}
