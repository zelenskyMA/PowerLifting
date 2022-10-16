using PowerLifting.Domain.Interfaces.Common.Operations;
using PowerLifting.Domain.Interfaces.UserData.Application;

namespace PowerLifting.Application.Administration.AdministrationCommands
{
    /// <summary>
    /// Apply block changes for selected user
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
