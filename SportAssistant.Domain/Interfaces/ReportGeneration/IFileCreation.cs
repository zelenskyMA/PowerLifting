using SportAssistant.Domain.Models.ReportGeneration;

namespace SportAssistant.Domain.Interfaces.ReportGeneration;

public interface IFileCreation
{
    /// <summary>
    /// Создание файла отчета в виде массива байтов
    /// </summary>
    /// <param name="report">Массив данных для формирования отчета по тренировочному плану</param>
    /// <returns></returns>
    byte[] Generate(ReportData report);
}
