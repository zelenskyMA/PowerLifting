using SportAssistant.Domain.DbModels.Basic;
using SportAssistant.Domain.Enums;
using SportAssistant.Domain.Interfaces.Common.Repositories;
using SportAssistant.Domain.Interfaces.Settings.Application;
using SportAssistant.Domain.Models.Basic;
using System.Reflection;

namespace SportAssistant.Application.Settings
{
    public class ProcessSettings : IProcessSettings
    {
        private readonly ICrudRepo<SettingsDb> _settingsRepository;

        public ProcessSettings(ICrudRepo<SettingsDb> settingsRepository)
        {
            _settingsRepository = settingsRepository;
        }

        public async Task<AppSettings> GetAsync()
        {
            var settingsDb = await _settingsRepository.GetAllAsync();
            var settings = new AppSettings();

            foreach (PropertyInfo prop in typeof(AppSettings).GetProperties())
            {
                var enumItem = (DictionarySettings)Enum.Parse(typeof(DictionarySettings), prop.Name);
                var value = settingsDb.FirstOrDefault(t => t.Id == (int)enumItem)?.Value ?? string.Empty;

                var propType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                if (propType == typeof(int))
                {
                    prop.SetValue(settings, int.Parse(string.IsNullOrEmpty(value) ? "0" : value), null);
                    continue;
                }

                prop.SetValue(settings, value, null);
            }

            return settings;
        }
    }
}
