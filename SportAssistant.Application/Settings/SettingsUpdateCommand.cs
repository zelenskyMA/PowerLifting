using SportAssistant.Domain.CustomExceptions;
using SportAssistant.Domain.DbModels.Basic;
using SportAssistant.Domain.Enums;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.Common.Repositories;
using SportAssistant.Domain.Interfaces.UserData.Application;
using SportAssistant.Domain.Models.Basic;
using System.Reflection;

namespace SportAssistant.Application.Settings
{
    /// <summary>
    /// Обновление настроек приложения
    /// </summary>
    public class SettingsUpdateCommand : ICommand<SettingsUpdateCommand.Param, bool>
    {
        private readonly IUserRoleCommands _userRoleCommands;
        private readonly ICrudRepo<SettingsDb> _settingsRepository;

        public SettingsUpdateCommand(
            IUserRoleCommands userRoleCommands,
            ICrudRepo<SettingsDb> settingsRepository)
        {
            _userRoleCommands = userRoleCommands;
            _settingsRepository = settingsRepository;
        }

        public async Task<bool> ExecuteAsync(Param param)
        {
            if (!await _userRoleCommands.IHaveRole(UserRoles.Admin))
            {
                throw new RoleException();
            }

            var settingsDb = await _settingsRepository.GetAllAsync();

            foreach (PropertyInfo prop in typeof(AppSettings).GetProperties())
            {
                var enumItem = (DictionarySettings)Enum.Parse(typeof(DictionarySettings), prop.Name);
                var value = param.Settings.GetType().GetProperty(prop.Name)?.GetValue(param.Settings, null) ?? string.Empty;

                var item = settingsDb.FirstOrDefault(t => t.Id == (int)enumItem);
                if (item != null)
                {
                    item.Value = value.ToString();
                }
            }

            _settingsRepository.UpdateList(settingsDb.ToList());

            return true;
        }

        public class Param
        {
            public AppSettings Settings { get; set; }
        }
    }
}
