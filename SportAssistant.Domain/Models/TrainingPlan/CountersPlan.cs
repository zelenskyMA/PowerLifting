using SportAssistant.Domain.Models.Common;

namespace SportAssistant.Domain.Models.TrainingPlan;

/// <summary>
/// Статистическая информация тренировочного плана. Все поля - расчетные суммы по всем дням.
/// </summary>
public class CountersPlan
{
    /// <summary>
    /// Количество упражнений по категориям (subType).
    /// </summary>
    public List<ValueEntity> CategoryCountersSum { get; set; } = new List<ValueEntity>();

    /// <summary>
    /// Сумма КПШ по категориям упражнений за день.
    /// </summary>
    public List<ValueEntity> LiftCountersByCategory { get; set; } = new List<ValueEntity>();

    /// <summary>
    /// Сумма Нагрузок по категориям упражнений за день.
    /// </summary>
    public List<ValueEntity> WeightLoadsByCategory { get; set; } = new List<ValueEntity>();

    /// <summary>
    /// Сумма Интенсивности по категориям упражнений за день.
    /// </summary>
    public List<ValueEntity> IntensitiesByCategory { get; set; } = new List<ValueEntity>();
}
