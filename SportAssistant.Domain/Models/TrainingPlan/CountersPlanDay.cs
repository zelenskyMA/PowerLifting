using SportAssistant.Domain.Models.Common;

namespace SportAssistant.Domain.Models.TrainingPlan;

/// <summary>
/// Статистическая информация тренировочного дня. Все поля расчетные.
/// </summary>
public class CountersPlanDay
{
    /// <summary>
    /// Сумма КПШ по всем упражнениям за день.
    /// </summary>
    public int LiftCounterSum { get; set; }

    /// <summary>
    /// Сумма Нагрузок по всем упражнениям за день.
    /// </summary>
    public int WeightLoadSum { get; set; }

    /// <summary>
    /// Сумма Интенсивности по всем упражнениям за день.
    /// </summary>
    public int IntensitySum { get; set; }

    /// <summary>
    /// Сумма КПШ по процентной интенсивности занятий.
    /// </summary>
    public List<LiftIntensity> LiftIntensities { get; set; } = new List<LiftIntensity>();

    /// <summary>
    /// Количество упражнений по подтипам.
    /// </summary>
    public List<ValueEntity> ExerciseTypeCounters { get; set; } = new List<ValueEntity>();


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
