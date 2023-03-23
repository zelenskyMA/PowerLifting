using Microsoft.Extensions.Configuration;
using SportAssistant.Application.UserData.Auth;
using SportAssistant.Domain.CustomExceptions;
using SportAssistant.Domain.DbModels.UserData;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.Common.Repositories;
using SportAssistant.Domain.Interfaces.UserData.Application;
using SportAssistant.Domain.Models.UserData.Auth;

namespace SportAssistant.Application.UserData.UserCommands
{
    /// <summary>
    /// New user registration
    /// </summary>
    public class UserRegisterCommand : ICommand<UserRegisterCommand.Param, TokenModel>
    {
        private readonly IProcessUser _processUser;
        private readonly ICrudRepo<UserDb> _userRepository;
        private readonly IConfiguration _configuration;
        private readonly JwtManager _jwtManager;

        public UserRegisterCommand(
            IProcessUser processUser,
            ICrudRepo<UserDb> userRepository,
            IConfiguration configuration)
        {
            _processUser = processUser;
            _userRepository = userRepository;
            _jwtManager = new JwtManager();
            _configuration = configuration;
        }

        /// <inheritdoc />
        public async Task<TokenModel> ExecuteAsync(Param param)
        {
            param.Login =  param.Login.Trim();

            _processUser.ValidateLogin(param.Login);
            _processUser.ValidatePassword(param.Password, param.PasswordConfirm, true);

            var userDb = (await _userRepository.FindAsync(t => t.Email == param.Login)).FirstOrDefault();
            if (userDb != null)
            {
                throw new BusinessException("Пользователь с указанным логином уже существует.");
            }

            var user = await _processUser.CreateNewUser(param.Login, param.Password);

            var token = new TokenModel()
            {
                Token = _jwtManager.CreateToken(_configuration, user),
                RefreshToken = _jwtManager.CreateRefreshToken(_configuration, user)
            };

            return token;
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
