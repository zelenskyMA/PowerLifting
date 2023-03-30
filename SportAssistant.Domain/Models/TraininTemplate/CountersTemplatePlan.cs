using SportAssistant.Domain.Models.Common;

namespace SportAssistant.Domain.Models.TraininTemplate;


/// <summary>
/// Статистическая информация шаблона тренировочного плана. Все поля - расчетные суммы по всем дням.
/// </summary>
public class CountersTemplatePlan
{
    /// <summary>
    /// Количество упражнений по категориям (subType).
    /// </summary>
    public List<ValueEntity> CategoryCountersSum { get; set; } = new List<ValueEntity>();

}
