using Microsoft.AspNetCore.Mvc;
using PowerLifting.Application.UserData.UserInfoCommands;
using PowerLifting.Domain.Interfaces.Administration;
using PowerLifting.Domain.Interfaces.Common.Actions;
using PowerLifting.Domain.Models.UserData;

namespace PowerLifting.Service.Controllers.Administration
{
    [Route("userAdministration")]
    public class UserAdministration : BaseController
    {
        private readonly IUserAdministrationCommands _userAdministrationCommands;

        public UserAdministration(IUserAdministrationCommands userAdministrationCommands)
        {
            _userAdministrationCommands = userAdministrationCommands;
        }

        [HttpGet]
        [Route("getCard")]
        public async Task<UserCard> GetCard([FromServices] ICommand<UserInfoGetCardQuery.Param, UserCard> command, string? login, int userId = 0)
        {
            var result = await command.ExecuteAsync(new UserInfoGetCardQuery.Param() { UserId = userId, Login = login });
            return result;
        }

        [HttpPost]
        [Route("applyRoles")]
        public async Task<bool> ApplyRoles(RolesInfo rolesInfo)
        {
            await _userAdministrationCommands.ApplyRolesAsync(rolesInfo);
            return true;
        }

        [HttpPost]
        [Route("applyBlock")]
        public async Task<bool> ApplyBlock(BlockInfo rolesInfo)
        {
            await _userAdministrationCommands.ApplyBlock(rolesInfo);
            return true;
        }
    }
}
