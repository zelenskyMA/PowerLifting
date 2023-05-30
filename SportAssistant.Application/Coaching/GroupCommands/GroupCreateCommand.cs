using SportAssistant.Application.UserData.Auth.Interfaces;
using SportAssistant.Domain.CustomExceptions;
using SportAssistant.Domain.DbModels.Coaching;
using SportAssistant.Domain.Enums;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.Common.Repositories;
using SportAssistant.Domain.Interfaces.UserData.Application;
using SportAssistant.Domain.Models.Coaching;

namespace SportAssistant.Application.Coaching.TrainingGroupCommands;

/// <summary>
/// Создание тренировочной группы тренером
/// </summary>
public class GroupCreateCommand : ICommand<GroupCreateCommand.Param, bool>
{
    private readonly IUserRoleCommands _userRoleCommands;
    private readonly ICrudRepo<TrainingGroupDb> _trainingGroupRepository;
    private readonly IUserProvider _user;

    public GroupCreateCommand(
     IUserRoleCommands userRoleCommands,
     ICrudRepo<TrainingGroupDb> trainingGroupRepository,
     IUserProvider user)
    {
        _trainingGroupRepository = trainingGroupRepository;
        _userRoleCommands = userRoleCommands;
        _user = user;
    }

    public async Task<bool> ExecuteAsync(Param param)
    {
        if (string.IsNullOrWhiteSpace(param.Group.Name))
        {
            throw new BusinessException($"Название группы обязательно");
        }

        if (!await _userRoleCommands.IHaveRole(UserRoles.Coach))
        {
            throw new RoleException();
        }

        var groupDb = await _trainingGroupRepository.FindAsync(t => t.Name == param.Group.Name && t.CoachId == _user.Id);
        if (groupDb.Any())
        {
            throw new BusinessException($"Группа с названием '{param.Group.Name}' уже существует");
        }

        await _trainingGroupRepository.CreateAsync(new TrainingGroupDb()
        {
            Name = param.Group.Name,
            Description = param.Group.Description,
            CoachId = _user.Id
        });

        return true;
    }

    public class Param
    {
        public TrainingGroup Group { get; set; }
    }
}
