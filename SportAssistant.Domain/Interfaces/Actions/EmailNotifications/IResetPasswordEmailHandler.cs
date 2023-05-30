namespace SportAssistant.Domain.Interfaces.Actions.EmailNotifications;

public interface IResetPasswordEmailHandler
{
    /// <summary>
    /// Отправка почтового сообщения действующему пользователю
    /// </summary>
    /// <param name="email">Адрес пользователя</param>
    /// <param name="password">Новый пароль</param>
    /// <returns></returns>
    Task HandleAsync(string email, string password);
}
