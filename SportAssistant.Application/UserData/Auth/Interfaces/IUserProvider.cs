namespace SportAssistant.Application.UserData.Auth.Interfaces
{
    /// <summary>
    /// Провайдер данный пользователя из JWT (передается при запросах), и из БД
    /// </summary>
    public interface IUserProvider
    {
        /// <summary>
        /// Ид пользователя
        /// </summary>
        int Id { get; }

        /// <summary>
        /// Почта (логин) пользователя
        /// </summary>
        string Email { get; }
    }
}
