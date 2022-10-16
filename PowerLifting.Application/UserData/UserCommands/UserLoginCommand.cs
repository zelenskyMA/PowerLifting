using AutoMapper;
using Microsoft.Extensions.Configuration;
using PowerLifting.Application.UserData.Auth;
using PowerLifting.Domain.Interfaces.Common.Operations;
using PowerLifting.Domain.Interfaces.UserData.Application;
using PowerLifting.Domain.Models.UserData;
using PowerLifting.Domain.Models.UserData.Auth;


namespace PowerLifting.Application.UserData.UserCommands.UserCommands
{
    /// <summary>
    /// Login user and generate jwt token
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
            _processUser.ValidateLogin(param.Login);
            _processUser.ValidatePassword(param.Password);

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
