using SportAssistant.Domain.Models.ReportGeneration;

namespace SportAssistant.Domain.Interfaces.ReportGeneration;

public interface IDataCollector
{
    /// <summary>
    /// Подготовка массива данных для формирования отчета по тренировочному плану
    /// </summary>
    /// <param name="planId">Ид плана</param>
    /// <param name="completedOnly">Флаг получения информации только по завершенным упражнениям</param>
    /// <returns></returns>
    Task<ReportData> CollectPlanData(int planId, bool completedOnly);
}
