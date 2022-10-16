namespace SportAssistant.Application.UserData.Auth.Interfaces
{
    /// <summary>
    /// Provider of user data from JWT, passed within authorization procedure
    /// </summary>
    public interface IUserProvider
    {
        /// <summary>
        /// User Id
        /// </summary>
        int Id { get; }

        /// <summary>
        /// User email (login)
        /// </summary>
        string Email { get; }
    }
}
