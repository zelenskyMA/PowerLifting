using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.Settings.Application;
using SportAssistant.Domain.Models.Basic;

namespace SportAssistant.Application.Settings
{
    /// <summary>
    /// Получение списка настроек приложения
    /// </summary>
    public class SettingsGetQuery : ICommand<SettingsGetQuery.Param, AppSettings>
    {
        private readonly IProcessSettings _processSettings;

        public SettingsGetQuery(
            IProcessSettings processSettings)
        {
            _processSettings = processSettings;
        }

        public async Task<AppSettings> ExecuteAsync(Param param)
        {
            var settings = await _processSettings.GetAsync();
            return settings;
        }

        public class Param { }
    }
}
