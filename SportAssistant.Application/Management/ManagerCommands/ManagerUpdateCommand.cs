using AutoMapper;
using SportAssistant.Application.UserData.Auth.Interfaces;
using SportAssistant.Domain.CustomExceptions;
using SportAssistant.Domain.DbModels.Management;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.Common.Repositories;
using SportAssistant.Domain.Interfaces.Management;
using SportAssistant.Domain.Models.Management;

namespace SportAssistant.Application.Management.ManagerCommands;

public class ManagerUpdateCommand : ICommand<ManagerUpdateCommand.Param, bool>
{
    private readonly IProcessOrgData _processOrgData;
    private readonly ICrudRepo<ManagerDb> _managerRepository;
    private readonly IMapper _mapper;
    private readonly IUserProvider _user;

    public ManagerUpdateCommand(
        IProcessOrgData processOrgData,
        ICrudRepo<ManagerDb> managerRepository,
        IMapper mapper,
        IUserProvider user)
    {
        _processOrgData = processOrgData;
        _managerRepository = managerRepository;
        _mapper = mapper;
        _user = user;
    }

    // <inheritdoc />
    public async Task<bool> ExecuteAsync(Param param)
    {
        var managerDb = await GetValidatedManager(param);

        _managerRepository.Update(managerDb);

        return true;
    }

    private async Task<ManagerDb> GetValidatedManager(Param param)
    {
        var manager = _mapper.Map<ManagerDb>(param.Manager);
        if (_user.Id == manager.UserId)
        {
            return manager; // сохрани сам
        }

        var org = await _processOrgData.GetOrgByUserIdAsync() ?? await _processOrgData.GetOrgByManagerIdAsync();
        if (org == null)
        {
            throw new RoleException(); // я не владелец орг
        }

        var managerDb = await _managerRepository.FindOneAsync(t => t.UserId == param.Manager.Id);
        if (managerDb == null || managerDb.OrgId != org.Id)
        {
            throw new BusinessException("Выбранный менеджер не найден");
        }

        if (managerDb?.AllowedCoaches < manager.AllowedCoaches)
        {
            var orgInfo = await _processOrgData.GetOrgInfoAsync(org.Id, org.MaxCoaches);
            var numberToAssign = manager.AllowedCoaches - managerDb.AllowedCoaches;
            if (orgInfo.LeftToDistribute < numberToAssign)
            {
                throw new BusinessException("У организации не хватает лицензий");
            }
        }

        return manager; // сохраняет владелец орг
    }

    public class Param
    {
        public Manager? Manager { get; set; }
    }
}
