using Microsoft.AspNetCore.Mvc;
using SportAssistant.Application.Administration.AdministrationCommands;
using SportAssistant.Application.UserData.UserInfoCommands;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Models.UserData;

namespace SportAssistant.Service.Controllers.Administration
{
    [Route("administration")]
    public class AdministrationController : BaseController
    {
        [HttpGet]
        [Route("getCard")]
        public async Task<UserCard> GetCardAsync([FromServices] ICommand<UserInfoGetCardQuery.Param, UserCard> command, string? login, int userId = 0)
        {
            var result = await command.ExecuteAsync(new UserInfoGetCardQuery.Param() { UserId = userId, Login = login });
            return result;
        }

        [HttpPost]
        [Route("applyRoles")]
        public async Task<bool> ApplyRolesAsync([FromServices] ICommand<ApplyRolesCommand.Param, bool> command, ApplyRolesCommand.Param param)
        {
            var result = await command.ExecuteAsync(param);
            return result;
        }

        [HttpPost]
        [Route("applyBlock")]
        public async Task<bool> ApplyBlockAsync([FromServices] ICommand<ApplyBlockCommand.Param, bool> command, ApplyBlockCommand.Param param)
        {
            var result = await command.ExecuteAsync(param);
            return result;
        }
    }
}
