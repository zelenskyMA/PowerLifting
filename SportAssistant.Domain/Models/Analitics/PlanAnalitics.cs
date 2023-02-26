namespace SportAssistant.Domain.Models.Analitics;

public class PlanAnalitics
{
    /// <summary>
    /// Сводные данные в разрезе плана, отсортированные по датам
    /// </summary>
    public List<PlanCounterAnalitics> PlanCounters { get; set; } = new List<PlanCounterAnalitics>();

    /// <summary>
    /// Сводные данные по подтипам упражнений в планах, отсортированные по датам
    /// </summary>
    public List<TypeCounterAnalitics> TypeCounters { get; set; } = new List<TypeCounterAnalitics>();

    /// <summary>
    /// Содержит записи для каждого плана в сете аналитики. Используется как полноценный датасет для контрола графиков.
    /// Основные данные будут получены из данных в TypeCounters.
    /// </summary>
    public List<DateValueModel> FullTypeCounterList { get; set; } = new List<DateValueModel>();
}
