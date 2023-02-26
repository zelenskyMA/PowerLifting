using SportAssistant.Application.UserData.Auth;
using SportAssistant.Domain.DbModels.UserData;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.Common.Repositories;
using SportAssistant.Domain.Interfaces.UserData.Application;

namespace SportAssistant.Application.UserData.UserCommands
{
    /// <summary>
    /// Изменение пользовательского пароля
    /// </summary>
    public class UserChangePasswordCommand : ICommand<UserChangePasswordCommand.Param, bool>
    {
        private readonly IProcessUser _processUser;
        private readonly PasswordManager _passwordManager;
        private readonly ICrudRepo<UserDb> _userRepository;

        public UserChangePasswordCommand(
            IProcessUser processUser,
            ICrudRepo<UserDb> userRepository)
        {
            _processUser = processUser;
            _userRepository = userRepository;
            _passwordManager = new PasswordManager();
        }

        /// <inheritdoc />
        public async Task<bool> ExecuteAsync(Param param)
        {
            _processUser.ValidateLogin(param.Login);
            _processUser.ValidatePassword(param.Password, param.PasswordConfirm, true);

            var userDb = await _processUser.TryToLogin(param.Login, param.OldPassword);
            userDb.Salt = _passwordManager.GenerateSalt();
            userDb.Password = _passwordManager.ApplySalt(param.Password, userDb.Salt);

            _userRepository.Update(userDb);

            return true;
        }

        public class Param
        {
            public string Login { get; set; } = string.Empty;

            public string? OldPassword { get; set; }

            public string Password { get; set; } = string.Empty;

            public string PasswordConfirm { get; set; } = string.Empty;
        }
    }
}
