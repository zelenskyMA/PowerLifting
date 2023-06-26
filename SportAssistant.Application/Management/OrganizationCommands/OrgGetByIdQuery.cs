using AutoMapper;
using SportAssistant.Application.Common.Actions;
using SportAssistant.Application.UserData.Auth.Interfaces;
using SportAssistant.Domain.DbModels.Management;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.Common.Repositories;
using SportAssistant.Domain.Interfaces.UserData.Application;

namespace SportAssistant.Application.Management.OrganizationCommands;

/// <summary>
/// Получение организации по ее Ид.
/// </summary>
public class OrgGetByIdQuery : ICommand<OrgGetByIdQuery.Param, Organization>
{
    private readonly IProcessUserInfo _processUserInfo;
    private readonly ICrudRepo<OrganizationDb> _orgRepository;
    private readonly IUserProvider _user;
    private readonly IMapper _mapper;

    public OrgGetByIdQuery(
        IProcessUserInfo processUserInfo,
        ICrudRepo<OrganizationDb> orgRepository,
        IUserProvider user,
        IMapper mapper)
    {
        _processUserInfo = processUserInfo;
        _orgRepository = orgRepository;
        _user = user;
        _mapper = mapper;
    }

    public async Task<Organization> ExecuteAsync(Param param)
    {
        var orgDb = param.Id == 0 ?
            await _orgRepository.FindOneAsync(t => t.OwnerId == _user.Id) :
            await _orgRepository.FindOneAsync(t => t.Id == param.Id);

        if (orgDb == null)
        {
            return new Organization();
        }

        var ownerInfo = await _processUserInfo.GetInfo(orgDb.OwnerId, false);

        var org = _mapper.Map<Organization>(orgDb);
        org.OwnerLegalName = UserNaming.GetLegalFullName(ownerInfo.FirstName, ownerInfo.Surname, ownerInfo.Patronimic, "Имя не задано");

        return org;
    }

    public class Param
    {
        public int Id { get; set; }
    }
}
