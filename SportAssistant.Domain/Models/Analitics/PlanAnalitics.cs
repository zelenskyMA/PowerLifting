namespace SportAssistant.Domain.Models.Analitics;

/// <summary>
/// DTO. Сет аналитики по планам из выборки.
/// </summary>
public class PlanAnalitics
{
    /// <summary>
    /// Ось Х графика. Содержит все даты выборки.
    /// </summary>
    public List<ChartDot> ChartDotsList { get; set; } = new List<ChartDot>();

    /// <summary>
    /// Сводные данные по категориям упражнений в планах.
    /// </summary>
    public List<ChartDataItem> CategoryCounters { get; set; } = new List<ChartDataItem>();

    /// <summary>
    /// Сводные данные КПШ по категориям упражнений в планах.
    /// </summary>
    public List<ChartDataItem> LiftCountersByCategory { get; set; } = new List<ChartDataItem>();

    /// <summary>
    /// Сводные данные Нагрузок по категориям упражнений в планах.
    /// </summary>
    public List<ChartDataItem> WeightLoadsByCategory { get; set; } = new List<ChartDataItem>();

    /// <summary>
    /// Сводные данные Интенсивности по категориям упражнений в планах.
    /// </summary>
    public List<ChartDataItem> IntensitiesByCategory { get; set; } = new List<ChartDataItem>();
}
