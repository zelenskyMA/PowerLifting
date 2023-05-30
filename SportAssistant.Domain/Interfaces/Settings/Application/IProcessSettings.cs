using SportAssistant.Domain.Models.Basic;

namespace SportAssistant.Domain.Interfaces.Settings.Application;

public interface IProcessSettings
{
    /// <summary>
    /// Получение объекта настроек приложения, полученного из списка в БД.
    /// </summary>
    /// <returns></returns>
    Task<AppSettings> GetAsync();
}
