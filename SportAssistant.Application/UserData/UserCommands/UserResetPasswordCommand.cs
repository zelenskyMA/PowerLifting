using SportAssistant.Application.UserData.Auth;
using SportAssistant.Domain.CustomExceptions;
using SportAssistant.Domain.DbModels.UserData;
using SportAssistant.Domain.Interfaces.Actions.EmailNotifications;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.Common.Repositories;

namespace SportAssistant.Application.UserData.UserCommands
{
    /// <summary>
    /// Сброс пользовательского пароля
    /// </summary>
    public class UserResetPasswordCommand : ICommand<UserResetPasswordCommand.Param, bool>
    {
        private readonly IResetPasswordEmailHandler _emailHandler;
        private readonly ICrudRepo<UserDb> _userRepository;
        private readonly PasswordManager _passwordManager;

        public UserResetPasswordCommand(
            IResetPasswordEmailHandler emailHandler,
            ICrudRepo<UserDb> userRepository)
        {
            _emailHandler = emailHandler;
            _userRepository = userRepository;
            _passwordManager = new PasswordManager();
        }

        /// <inheritdoc />
        public async Task<bool> ExecuteAsync(Param param)
        {
            param.Login = param.Login.Trim();

            var userDb = (await _userRepository.FindAsync(t => t.Email == param.Login)).FirstOrDefault();
            if (userDb == null)
            {
                throw new BusinessException("Пользователь с указанным логином не найден.");
            }

            if (userDb.Blocked)
            {
                throw new BusinessException($"Пользователь заблокирован, обратитесь к администрации.'");
            }

            var password = _passwordManager.GeneratePassword();
            userDb.Salt = _passwordManager.GenerateSalt();
            userDb.Password = _passwordManager.ApplySalt(password, userDb.Salt);

            await _emailHandler.HandleAsync(param.Login, password);

            _userRepository.Update(userDb);

            return true;
        }

        public class Param
        {
            public string Login { get; set; } = string.Empty;
        }
    }
}
