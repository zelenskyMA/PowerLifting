using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.UserData.Application;

namespace SportAssistant.Application.Administration.AdministrationCommands
{
    /// <summary>
    /// Изменение статуса блокировки для выбранного пользователя.
    /// </summary>
    public class ApplyBlockCommand : ICommand<ApplyBlockCommand.Param, bool>
    {
        private readonly IUserBlockCommands _userBlockCommands;

        public ApplyBlockCommand(IUserBlockCommands userBlockCommands)
        {
            _userBlockCommands = userBlockCommands;
        }

        /// <inheritdoc />
        public async Task<bool> ExecuteAsync(Param param)
        {
            if (param.Status)
            {
                await _userBlockCommands.BlockUser(param.UserId, param.Reason);
            }
            else
            {
                await _userBlockCommands.UnblockUser(param.UserId);
            }

            return true;
        }

        public class Param
        {
            public int UserId { get; set; }

            public bool Status { get; set; }

            public string? Reason { get; set; }
        }
    }
}
