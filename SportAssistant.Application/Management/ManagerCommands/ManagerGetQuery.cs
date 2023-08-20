using AutoMapper;
using SportAssistant.Application.UserData.Auth.Interfaces;
using SportAssistant.Domain.CustomExceptions;
using SportAssistant.Domain.DbModels.Management;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.Common.Repositories;
using SportAssistant.Domain.Interfaces.Management;
using SportAssistant.Domain.Interfaces.UserData.Application;
using SportAssistant.Domain.Models.Management;

namespace SportAssistant.Application.Management.ManagerCommands;

public class ManagerGetQuery : ICommand<ManagerGetQuery.Param, Manager>
{
    private readonly IProcessUserInfo _processUserInfo;
    private readonly IProcessOrgDataByUserId _processOrgDataByUserId;
    private readonly ICrudRepo<ManagerDb> _managerRepository;
    private readonly IMapper _mapper;
    private readonly IUserProvider _user;

    public ManagerGetQuery(
        IProcessUserInfo processUserInfo,
        IProcessOrgDataByUserId processOrgDataByUserId,
        ICrudRepo<ManagerDb> managerRepository,
        IMapper mapper,
        IUserProvider user)
    {
        _processUserInfo = processUserInfo;
        _processOrgDataByUserId = processOrgDataByUserId;
        _managerRepository = managerRepository;
        _mapper = mapper;
        _user = user;
    }

    /// <inheritdoc />
    public async Task<Manager> ExecuteAsync(Param param)
    {
        var managerDb = await _managerRepository.FindOneAsync(t => t.UserId == param.Id);
        if (managerDb == null)
        {
            throw new BusinessException("Учетка менеджера не найдена");
        }

        if (!await CheckViewRights(param, managerDb))
        {
            throw new RoleException();
        }

        var manager = _mapper.Map<Manager>(managerDb);

        var info = await _processUserInfo.GetInfoList(new List<int>() { managerDb.UserId });
        manager.Name = info.FirstOrDefault()?.LegalName;

        return manager;
    }

    private async Task<bool> CheckViewRights(Param param, ManagerDb manager)
    {
        if (param.Id == _user.Id)
        {
            return true; // просмотр самим собой
        }

        var org = await _processOrgDataByUserId.GetOrgByUserIdAsync();
        if (org?.Id == manager.OrgId)
        {
            return true; // просмотр владельцем орг.
        }

        return false;
    }

    public class Param
    {
        public int Id { get; set; }
    }
}
