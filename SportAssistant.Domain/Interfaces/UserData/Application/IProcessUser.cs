using SportAssistant.Domain.DbModels.UserData;
using SportAssistant.Domain.Models.UserData;

namespace SportAssistant.Domain.Interfaces.UserData.Application;

public interface IProcessUser
{
    /// <summary>
    /// User attempt to login
    /// </summary>
    /// <param name="login">Login</param>
    /// <param name="password">Password</param>
    /// <returns></returns>
    Task<UserDb> TryToLogin(string login, string password);

    /// <summary>
    /// Create new app user
    /// </summary>
    /// <param name="login">Login</param>
    /// <param name="password">Password</param>
    /// <returns></returns>
    Task<UserModel> CreateNewUser(string login, string password);

    /// <summary>
    /// Validate user login
    /// </summary>
    /// <param name="login">Login</param>
    void ValidateLogin(string login);

    /// <summary>
    /// Validate user password
    /// </summary>
    /// <param name="password">Password</param>
    /// <param name="confirm">Confirmation password</param>
    /// <param name="checkConfirmation">Check confirmation</param>
    void ValidatePassword(string password, string confirm = "", bool checkConfirmation = false);
}
