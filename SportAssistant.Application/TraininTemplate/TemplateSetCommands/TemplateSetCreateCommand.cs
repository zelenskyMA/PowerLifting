using SportAssistant.Application.UserData.Auth.Interfaces;
using SportAssistant.Domain.CustomExceptions;
using SportAssistant.Domain.DbModels.TrainingTemplate;
using SportAssistant.Domain.Enums;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.Common.Repositories;
using SportAssistant.Domain.Interfaces.UserData.Application;

namespace SportAssistant.Application.TrainingTemplate.TemplateSetCommands;

/// <summary>
/// Создание тренировочного цикла
/// </summary>
public class TemplateSetCreateCommand : ICommand<TemplateSetCreateCommand.Param, bool>
{
    private readonly IUserRoleCommands _userRoleCommands;
    private readonly ICrudRepo<TemplateSetDb> _templateSetRepository;
    private readonly IUserProvider _user;

    public TemplateSetCreateCommand(
        IUserRoleCommands userRoleCommands,
        ICrudRepo<TemplateSetDb> templateSetRepository,
        IUserProvider user)
    {
        _userRoleCommands = userRoleCommands;
        _templateSetRepository = templateSetRepository;
        _user = user;
    }

    public async Task<bool> ExecuteAsync(Param param)
    {
        if (!await _userRoleCommands.IHaveRole(UserRoles.Coach))
        {
            throw new RoleException();
        }

        if (string.IsNullOrWhiteSpace(param.Name))
        {
            throw new BusinessException("Необходимо указать название цикла");
        }

        var templateSetDb = await _templateSetRepository.FindOneAsync(t => t.Name == param.Name && t.CoachId == _user.Id);
        if (templateSetDb != null)
        {
            throw new BusinessException("Тренировочный цикл с указанным именем уже существует");
        }

        var templateSet = new TemplateSetDb()
        {
            Name = param.Name,
            CoachId = _user.Id,
        };

        await _templateSetRepository.CreateAsync(templateSet);

        return true;
    }

    public class Param
    {
        public string Name { get; set; }
    }
}
