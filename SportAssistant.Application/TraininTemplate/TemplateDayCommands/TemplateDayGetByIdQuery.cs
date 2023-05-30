using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.TrainingTemplate.Application;
using SportAssistant.Domain.Models.TrainingTemplate;

namespace SportAssistant.Application.TrainingTemplate.TemplateDayCommands;

/// <summary>
/// Получение тренировочного дня из шаблона по Ид.
/// </summary>
public class TemplateDayGetByIdQuery : ICommand<TemplateDayGetByIdQuery.Param, TemplateDay>
{
    private readonly IProcessTemplateDay _processTemplateDay;
    private readonly IProcessTemplateSet _processTemplateSet;
    private readonly IProcessSetUserId _processSetUserId;

    public TemplateDayGetByIdQuery(
        IProcessTemplateDay processTemplateDay,
        IProcessTemplateSet processTemplateSet,
        IProcessSetUserId processSetUserId)
    {
        _processTemplateDay = processTemplateDay;
        _processTemplateSet = processTemplateSet;
        _processSetUserId = processSetUserId;
    }

    public async Task<TemplateDay> ExecuteAsync(Param param)
    {
        var templateDay = await _processTemplateDay.GetAsync(param.Id);

        if (templateDay != null) // Сохраняем возможность возвращать null, когда ничего не нашли.
        {
            var ownerId = await _processSetUserId.GetByDayId(param.Id);
            await _processTemplateSet.ViewAllowedForDataOfUserAsync(ownerId);
        }

        return templateDay;
    }

    public class Param
    {
        public int Id { get; set; }
    }
}
