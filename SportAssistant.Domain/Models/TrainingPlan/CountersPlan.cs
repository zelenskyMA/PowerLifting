using SportAssistant.Domain.Models.Common;

namespace SportAssistant.Domain.Models.TrainingPlan;

/// <summary>
/// Статистическая информация тренировочного плана. Все поля - расчетные суммы по всем дням.
/// </summary>
public class CountersPlan
{
    /// <summary>
    /// Сумма КПШ по всем упражнениям за все дни плана.
    /// </summary>
    public int LiftCounterSum { get; set; }

    /// <summary>
    /// Сумма Нагрузок по всем упражнениям за все дни плана.
    /// </summary>
    public int WeightLoadSum { get; set; }

    /// <summary>
    /// Сумма Интенсивности по всем упражнениям за все дни плана.
    /// </summary>
    public int IntensitySum { get; set; }

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
