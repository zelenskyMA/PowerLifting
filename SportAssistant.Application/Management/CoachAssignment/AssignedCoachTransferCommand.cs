using SportAssistant.Application.UserData.Auth.Interfaces;
using SportAssistant.Domain.CustomExceptions;
using SportAssistant.Domain.DbModels.Management;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.Common.Repositories;
using SportAssistant.Domain.Interfaces.Management;
using SportAssistant.Domain.Interfaces.UserData.Application;
using SportAssistant.Domain.Models.Management;

namespace SportAssistant.Application.Management.CoachAssignment;

public class AssignedCoachTransferCommand : ICommand<AssignedCoachTransferCommand.Param, bool>
{
    private readonly IProcessManager _processManager;
    private readonly IProcessUserInfo _processUserInfo;
    private readonly ICrudRepo<AssignedCoachDb> _assignedCoachRepository;
    private readonly IUserProvider _user;

    public AssignedCoachTransferCommand(
        IProcessManager processManager,
        IProcessUserInfo processUserInfo,
        ICrudRepo<AssignedCoachDb> assignedCoachRepository,
        IUserProvider user)
    {
        _processManager = processManager;
        _processUserInfo = processUserInfo;
        _assignedCoachRepository = assignedCoachRepository;
        _user = user;
    }

    /// <inheritdoc />
    public async Task<bool> ExecuteAsync(Param param)
    {
        var manager = await _processManager.GetBaseAsync(_user.Id);
        if (manager == null)
        {
            throw new RoleException();
        }

        var targetManager = await _processManager.GetBaseAsync(param.TargetManagerId);
        if (targetManager == null || manager.OrgId != targetManager.OrgId)
        {
            throw new BusinessException("Указанный пользователь, не менеджер вашей организации");
        }

        var coachesCount = await _assignedCoachRepository.CountAsync(t => t.ManagerId == param.TargetManagerId);
        if (targetManager.AllowedCoaches <= coachesCount)
        {
            throw new BusinessException("У выбранного менеджера недостаточно лицензий");
        }

        var coachDb = await ValidateCoach(param.CoachId, manager);
        coachDb.ManagerId = targetManager.Id;
        _assignedCoachRepository.Update(coachDb);

        return true;
    }

    private async Task<AssignedCoachDb> ValidateCoach(int coachId, Manager manager)
    {
        var coachDb = await _assignedCoachRepository.FindOneAsync(t => t.CoachId == coachId);
        if (coachDb == null)
        {
            throw new BusinessException("Указанный пользователь не ваш тренер");
        }

        if (coachDb.ManagerId != manager.Id)
        {
            var info = await _processUserInfo.GetInfoList(new List<int>() { coachDb.ManagerId });
            throw new BusinessException($"Указанный пользователь - тренер менеджера {info.FirstOrDefault()?.LegalName}");
        }

        return coachDb;
    }


    public class Param
    {
        public int CoachId { get; set; }

        public int TargetManagerId { get; set; }
    }
}
