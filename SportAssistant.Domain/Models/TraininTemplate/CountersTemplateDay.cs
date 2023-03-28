using SportAssistant.Domain.Models.Common;

namespace SportAssistant.Domain.Models.TraininTemplate;

/// <summary>
/// Статистическая информация шаблона тренировочного дня. Все поля расчетные.
/// </summary>
public class CountersTemplateDay
{
    /// <summary>
    /// Сумма КПШ по всем упражнениям за день.
    /// </summary>
    public int LiftCounterSum { get; set; }

    /// <summary>
    /// Суммарный процент Нагрузок по всем упражнениям за день.
    /// </summary>
    public int WeightLoadPercentageSum { get; set; }

    /// <summary>
    /// Количество упражнений по подтипам.
    /// </summary>
    public List<ValueEntity> ExerciseTypeCounters { get; set; } = new List<ValueEntity>();
}
