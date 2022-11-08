using SportAssistant.Domain.CustomExceptions;
using SportAssistant.Domain.DbModels.TraininTemplate;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.Common.Repositories;
using SportAssistant.Infrastructure.DataContext;

namespace SportAssistant.Application.TraininTemplate.TemplatePlanCommands
{
    /// <summary>
    /// Создание шаблона тренировочного плана
    /// </summary>
    public class TemplatePlanCreateCommand : ICommand<TemplatePlanCreateCommand.Param, int>
    {
        private readonly ICrudRepo<TemplatePlanDb> _templatePlanRepository;
        private readonly ICrudRepo<TemplateDayDb> _templateDayRepository;
        private readonly IContextProvider _provider;

        public TemplatePlanCreateCommand(
            ICrudRepo<TemplatePlanDb> templatePlanRepository,
            ICrudRepo<TemplateDayDb> templateDayRepository,
            IContextProvider provider)
        {
            _templatePlanRepository = templatePlanRepository;
            _templateDayRepository = templateDayRepository;
            _provider = provider;
        }

        public async Task<int> ExecuteAsync(Param param)
        {
            if (string.IsNullOrWhiteSpace(param.Name))
            {
                throw new BusinessException("Необходимо указать название шаблона");
            }

            var templatePlanDb = await _templatePlanRepository.FindOneAsync(t => t.Name == param.Name);
            if (templatePlanDb != null)
            {
                throw new BusinessException("Тренировочный шаблон с указанным именем уже существует");
            }

            templatePlanDb = new TemplatePlanDb()
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

        public class Param
        {
            public string Name { get; set; }

            public int SetId { get; set; }
        }
    }
}
