using SportAssistant.Domain.Enums;
using SportAssistant.Domain.Interfaces.Actions.EmailNotifications;

namespace SportAssistant.Application.Common.Actions.EmailNotifications
{
    public class ResetPasswordEmailHandler : IResetPasswordEmailHandler
    {
        private readonly IProcessEmail _processEmail;

        public ResetPasswordEmailHandler(
            IProcessEmail processEmail)
        {
            _processEmail = processEmail;
        }

        /// <inheritdoc />
        public async Task HandleAsync(string email, string password)
        {
            var message = await _processEmail.GetMessage(EmailMessageTypes.ResetPassword);

            message.Body = string.Format(message.Body, password);
            message.Address = email;

            await _processEmail.SendAsync(message);
        }
    }
}
