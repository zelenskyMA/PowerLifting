using Microsoft.AspNetCore.Mvc;
using SportAssistant.Application.Settings;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Models.Basic;

namespace SportAssistant.Service.Controllers.Administration
{
    [Route("appSettings")]
    public class SettingsController : BaseController
    {
        [HttpGet]
        [Route("get")]
        public async Task<AppSettings> GetAsync([FromServices] ICommand<SettingsGetQuery.Param, AppSettings> command)
        {
            var result = await command.ExecuteAsync(new SettingsGetQuery.Param() { });
            return result;
        }

        [HttpPost]
        [Route("update")]
        public async Task<bool> UpdateAsync([FromServices] ICommand<SettingsUpdateCommand.Param, bool> command, AppSettings settings)
        {
            var result = await command.ExecuteAsync(new SettingsUpdateCommand.Param() { Settings = settings });
            return result;
        }

    }
}
