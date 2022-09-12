using PowerLifting.Domain.Models.Auth;
using PowerLifting.Domain.Models.UserData.Auth;

namespace PowerLifting.Domain.Interfaces.UserData.Application
{
    public interface IUserCommands
    {
        /// <summary>
        /// Login user and generate jwt token
        /// </summary>
        /// <param name="loginAuth">Credentials model for authentication</param>
        /// <returns>Jwt token</returns>
        Task<string> LoginAsync(LoginModel loginAuth);

        /// <summary>
        /// New user registration
        /// </summary>
        /// <param name="registerAuth">Credentials model for new user</param>
        /// <returns>Jwt token</returns>
        Task<string> RegisterAsync(RegistrationModel registerAuth);

        /// <summary>
        /// User password change
        /// </summary>
        /// <param name="registerAuth">Credentials model for new user password</param>
        /// <returns>Jwt token</returns>
        Task<string> ChangePasswordAsync(RegistrationModel registerAuth);
    }
}
