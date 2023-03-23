using AutoMapper;
using Microsoft.Extensions.Configuration;
using SportAssistant.Application.UserData.Auth;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.UserData.Application;
using SportAssistant.Domain.Models.UserData;
using SportAssistant.Domain.Models.UserData.Auth;


namespace SportAssistant.Application.UserData.UserCommands.UserCommands
{
    /// <summary>
    /// Аутентификация и авторизация пользователя. Генерация jwt токена.
    /// </summary>
    public class UserLoginCommand : ICommand<UserLoginCommand.Param, TokenModel>
    {
        private readonly IProcessUser _processUser;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly JwtManager _jwtManager;

        public UserLoginCommand(
            IProcessUser processUser,
            IConfiguration configuration,
            IMapper mapper)
        {
            _processUser = processUser;
            _jwtManager = new JwtManager();
            _configuration = configuration;
            _mapper = mapper;
        }

        /// <inheritdoc />
        public async Task<TokenModel> ExecuteAsync(Param param)
        {
            param.Login = param.Login.Trim();

            var userDb = await _processUser.TryToLogin(param.Login, param.Password);

            var user = _mapper.Map<UserModel>(userDb);
            var token = new TokenModel()
            {
                Token = _jwtManager.CreateToken(_configuration, user),
                RefreshToken = _jwtManager.CreateRefreshToken(_configuration, user)
            };

            return token;
        }

        public class Param
        {
            public string Login { get; set; }

            public string Password { get; set; }
        }
    }
}
