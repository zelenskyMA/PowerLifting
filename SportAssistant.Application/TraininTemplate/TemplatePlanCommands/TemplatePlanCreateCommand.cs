using SportAssistant.Application.TraininTemplate.TemplateSetCommands;
using SportAssistant.Domain.CustomExceptions;
using SportAssistant.Domain.DbModels.TrainingTemplate;
using SportAssistant.Domain.Enums;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.Common.Repositories;
using SportAssistant.Domain.Interfaces.TrainingTemplate.Application;
using SportAssistant.Domain.Interfaces.UserData.Application;
using SportAssistant.Infrastructure.DataContext;
using SportAssistant.Infrastructure.Repositories.TrainingTemplate;

namespace SportAssistant.Application.TrainingTemplate.TemplatePlanCommands
{
    /// <summary>
    /// Создание шаблона тренировочного плана
    /// </summary>
    public class TemplatePlanCreateCommand : ICommand<TemplatePlanCreateCommand.Param, int>
    {
        private readonly IUserRoleCommands _userRoleCommands;
        private readonly IProcessTemplateSet _processTemplateSet;
        private readonly IProcessSetUserId _processSetUserId;
        private readonly ICrudRepo<TemplatePlanDb> _templatePlanRepository;
        private readonly ICrudRepo<TemplateDayDb> _templateDayRepository;
        private readonly IContextProvider _provider;

        public TemplatePlanCreateCommand(
            IUserRoleCommands userRoleCommands,
            IProcessTemplateSet processTemplateSet,
            IProcessSetUserId processSetUserId,
            ICrudRepo<TemplatePlanDb> templatePlanRepository,
            ICrudRepo<TemplateDayDb> templateDayRepository,
            IContextProvider provider)
        {
            _userRoleCommands = userRoleCommands;
            _processTemplateSet = processTemplateSet;
            _processSetUserId = processSetUserId;
            _templatePlanRepository = templatePlanRepository;
            _templateDayRepository = templateDayRepository;
            _provider = provider;
        }

        public async Task<int> ExecuteAsync(Param param)
        {
            await VerifyRequestAsync(param.Name, param.SetId);

            var templatePlanDb = new TemplatePlanDb()
            {
                TemplateSetId = param.SetId,
                Name = param.Name,
            };

            await _templatePlanRepository.CreateAsync(templatePlanDb);
            await _provider.AcceptChangesAsync();

            for (int i = 0; i < 7; i++) // 7 дней в плане. Завязано в ui
            {
                var templateDay = new TemplateDayDb() { TemplatePlanId = templatePlanDb.Id, DayNumber = i + 1 };
                await _templateDayRepository.CreateAsync(templateDay);
            }

            return templatePlanDb.Id;
        }

        private async Task VerifyRequestAsync(string name, int setId)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new BusinessException("Необходимо указать название шаблона");
            }

            if (!await _userRoleCommands.IHaveRole(UserRoles.Coach))
            {
                throw new RoleException();
            }

            var templatePlanDb = await _templatePlanRepository.FindOneAsync(t => t.Name == name);
            if (templatePlanDb != null)
            {
                throw new BusinessException("Тренировочный шаблон с указанным именем уже существует");
            }

            var ownerId = await _processSetUserId.GetBySetId(setId);
            await _processTemplateSet.ChangingAllowedForUserAsync(ownerId);
        }

        public class Param
        {
            public string Name { get; set; }

            public int SetId { get; set; }
        }
    }
}
