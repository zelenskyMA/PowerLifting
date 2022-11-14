using SportAssistant.Domain.Enums;
using SportAssistant.Domain.Models.Basic;

namespace SportAssistant.Domain.Interfaces.Actions.EmailNotifications
{
    public interface IProcessEmail
    {
        /// <summary>
        /// Отправка почтового сообщения действующему пользователю
        /// </summary>
        /// <param name="message">Сообщение для отправки</param>
        /// <returns></returns>
        void Send(EmailMessage message);

        /// <summary>
        /// Получение шаблона почтового сообщения
        /// </summary>
        /// <param name="messageId">Ид сообщения</param>
        /// <returns></returns>
        Task<EmailMessage> GetMessage(EmailMessageTypes messageId);
    }
}
