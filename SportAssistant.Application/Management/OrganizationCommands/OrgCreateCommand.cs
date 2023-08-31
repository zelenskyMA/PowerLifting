using AutoMapper;
using SportAssistant.Application.UserData.Auth.Interfaces;
using SportAssistant.Domain.CustomExceptions;
using SportAssistant.Domain.DbModels.Management;
using SportAssistant.Domain.Enums;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.Common.Repositories;
using SportAssistant.Domain.Interfaces.UserData.Application;

namespace SportAssistant.Application.Management.OrganizationCommands;

/// <summary>
/// Создание организации
/// </summary>
public class OrgCreateCommand : ICommand<OrgCreateCommand.Param, bool>
{
    private readonly IUserRoleCommands _userRoleCommands;
    private readonly ICrudRepo<OrganizationDb> _orgRepository;
    private readonly IUserProvider _user;
    private readonly IMapper _mapper;

    public OrgCreateCommand(
     IUserRoleCommands userRoleCommands,
     ICrudRepo<OrganizationDb> orgRepository,
     IUserProvider user,
     IMapper mapper)
    {
        _userRoleCommands = userRoleCommands;
        _orgRepository = orgRepository;
        _user = user;
        _mapper = mapper;
    }

    public async Task<bool> ExecuteAsync(Param param)
    {
        if (string.IsNullOrWhiteSpace(param.Organization.Name))
        {
            throw new BusinessException($"Название организации обязательно");
        }

        if (!await _userRoleCommands.IHaveRole(UserRoles.Admin))
        {
            throw new RoleException();
        }

        var orgDb = await _orgRepository.FindOneAsync(t => t.Name == param.Organization.Name);
        if (orgDb != null)
        {
            throw new BusinessException($"Компания с названием '{param.Organization.Name}' уже существует");
        }

        orgDb = _mapper.Map<OrganizationDb>(param.Organization);
        orgDb.Id = 0;
        orgDb.OwnerId = orgDb.OwnerId == 0 ? _user.Id : orgDb.OwnerId;

        await _orgRepository.CreateAsync(orgDb);
        await _userRoleCommands.AddRole(orgDb.OwnerId, UserRoles.OrgOwner);

        return true;
    }

    public class Param
    {
        public Organization Organization { get; set; }
    }
}
