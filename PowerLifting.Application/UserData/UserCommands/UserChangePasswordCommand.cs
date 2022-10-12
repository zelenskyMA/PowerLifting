using PowerLifting.Application.UserData.Auth;
using PowerLifting.Domain.DbModels.UserData;
using PowerLifting.Domain.Interfaces.Common.Actions;
using PowerLifting.Domain.Interfaces.Common.Repositories;
using PowerLifting.Domain.Interfaces.UserData.Application;

namespace PowerLifting.Application.UserData.UserCommands
{
    /// <summary>
    /// User password change
    /// </summary>
    public class UserChangePasswordCommand : ICommand<UserChangePasswordCommand.Param, bool>
    {
        private readonly IProcessUser _processUser;
        private readonly PasswordManager _passwordManager;
        private readonly ICrudRepo<UserDb> _userRepository;

        public UserChangePasswordCommand(
            IProcessUser processUser,
            ICrudRepo<UserDb> userRepository,
            PasswordManager passwordManager)
        {
            _processUser = processUser;
            _userRepository = userRepository;
            _passwordManager = passwordManager;
        }

        /// <inheritdoc />
        public async Task<bool> ExecuteAsync(Param param)
        {
            _processUser.ValidateLogin(param.Login);
            _processUser.ValidatePassword(param.Password, param.PasswordConfirm, true);

            var userDb = await _processUser.TryToLogin(param.Login, param.OldPassword);

            string salt = _passwordManager.GenerateSalt();
            userDb.Salt = salt;
            userDb.Password = _passwordManager.ApplySalt(param.Password, salt);

            _userRepository.Update(userDb);

            return true;
        }

        public class Param
        {
            public string Login { get; set; }

            public string? OldPassword { get; set; }

            public string Password { get; set; }

            public string PasswordConfirm { get; set; }
        }
    }
}
