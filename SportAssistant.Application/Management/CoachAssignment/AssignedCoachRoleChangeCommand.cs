using SportAssistant.Application.UserData.Auth.Interfaces;
using SportAssistant.Domain.CustomExceptions;
using SportAssistant.Domain.DbModels.Management;
using SportAssistant.Domain.Enums;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.Common.Repositories;
using SportAssistant.Domain.Interfaces.Management;
using SportAssistant.Domain.Interfaces.UserData.Application;

namespace SportAssistant.Application.Management.CoachAssignment;

public class AssignedCoachRoleChangeCommand : ICommand<AssignedCoachRoleChangeCommand.Param, bool>
{
    private readonly IUserRoleCommands _userRoleCommands;
    private readonly IProcessUserInfo _processUserInfo;
    private readonly IProcessManager _processManager;
    private readonly ICrudRepo<AssignedCoachDb> _assignedCoachRepository;
    private readonly IUserProvider _user;

    public AssignedCoachRoleChangeCommand(
        IUserRoleCommands userRoleCommands,
        IProcessUserInfo processUserInfo,
        IProcessManager processManager,
        ICrudRepo<AssignedCoachDb> assignedCoachRepository,
        IUserProvider user)
    {
        _userRoleCommands = userRoleCommands;
        _processUserInfo = processUserInfo;
        _processManager = processManager;
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

        return param.RoleStatus ?
            await AddCoachStatus(param.UserId) :
            await RemoveCoachStatus(param.UserId);
    }

    private async Task<bool> AddCoachStatus(int userId)
    {
        var coachInfo = await _assignedCoachRepository.FindOneAsync(t => t.CoachId == userId);
        if (coachInfo != null)
        {
            if (coachInfo.ManagerId == _user.Id)
            {
                throw new BusinessException("Указанный пользователь уже ваш тренер");
            }

            var info = await _processUserInfo.GetInfoList(new List<int>() { coachInfo.ManagerId });
            throw new BusinessException($"Указанный пользователь - тренер менеджера {info.FirstOrDefault()?.LegalName}");
        }

        await _userRoleCommands.AddRole(userId, UserRoles.Coach);

        await _assignedCoachRepository.CreateAsync(new AssignedCoachDb()
        {
            CoachId = userId,
            ManagerId = _user.Id,
        });

        return true;
    }

    private async Task<bool> RemoveCoachStatus(int userId)
    {
        var coachInfo = await _assignedCoachRepository.FindOneAsync(t => t.CoachId == userId);
        if (coachInfo == null)
        {
            throw new BusinessException("Указанный пользователь не тренер");
        }

        if (coachInfo.ManagerId != _user.Id)
        {
            var info = await _processUserInfo.GetInfoList(new List<int>() { coachInfo.ManagerId });
            throw new BusinessException($"Указанный пользователь - тренер менеджера {info.FirstOrDefault()?.LegalName}");
        }

        await _userRoleCommands.RemoveRole(userId, UserRoles.Coach);

        _assignedCoachRepository.Delete(coachInfo);

        return true;
    }

    public class Param
    {
        public int UserId { get; set; }

        public bool RoleStatus { get; set; }
    }
}
