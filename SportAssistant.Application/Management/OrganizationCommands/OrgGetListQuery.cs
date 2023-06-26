using AutoMapper;
using SportAssistant.Domain.CustomExceptions;
using SportAssistant.Domain.DbModels.Management;
using SportAssistant.Domain.Enums;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.Common.Repositories;
using SportAssistant.Domain.Interfaces.UserData.Application;

namespace SportAssistant.Application.Management.OrganizationCommands;

/// <summary>
/// Получение списка организаций.
/// </summary>
public class OrgGetListQuery : ICommand<OrgGetListQuery.Param, List<Organization>>
{
    private readonly IUserRoleCommands _userRoleCommands;
    private readonly IProcessUserInfo _processUserInfo;
    private readonly ICrudRepo<OrganizationDb> _orgRepository;
    private readonly IMapper _mapper;

    public OrgGetListQuery(
        IUserRoleCommands userRoleCommands,
        IProcessUserInfo processUserInfo,
        ICrudRepo<OrganizationDb> orgRepository,
        IMapper mapper)
    {
        _userRoleCommands = userRoleCommands;
        _processUserInfo = processUserInfo;
        _orgRepository = orgRepository;
        _mapper = mapper;
    }

    public async Task<List<Organization>> ExecuteAsync(Param param)
    {
        if (!await _userRoleCommands.IHaveRole(UserRoles.Admin))
        {
            throw new RoleException();
        }

        var orgListDb = await _orgRepository.GetAllAsync();
        var orgList = orgListDb.Select(_mapper.Map<Organization>).ToList();

        var infoList = await _processUserInfo.GetInfoList(orgList.Select(t => t.OwnerId).ToList());
        foreach (var org in orgList)
        {
            org.OwnerLegalName = infoList.FirstOrDefault(t => t.Id == org.OwnerId)?.LegalName;
        }

        return orgList;
    }

    public class Param
    {
    }
}
