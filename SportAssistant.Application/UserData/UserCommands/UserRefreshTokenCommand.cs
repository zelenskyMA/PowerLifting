using AutoMapper;
using Microsoft.Extensions.Configuration;
using SportAssistant.Application.UserData.Auth;
using SportAssistant.Application.UserData.Auth.Interfaces;
using SportAssistant.Domain.CustomExceptions;
using SportAssistant.Domain.DbModels.UserData;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.Common.Repositories;
using SportAssistant.Domain.Models.UserData;
using SportAssistant.Domain.Models.UserData.Auth;

namespace SportAssistant.Application.UserData.UserCommands
{
    /// <summary>
    /// Refresh access token on expiration
    /// </summary>
    public class UserRefreshTokenCommand : ICommand<UserRefreshTokenCommand.Param, TokenModel>
    {
        private readonly ICrudRepo<UserDb> _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IUserProvider _user;
        private readonly IMapper _mapper;
        private readonly JwtManager _jwtManager;

        public UserRefreshTokenCommand(
            ICrudRepo<UserDb> userRepository,
            IConfiguration configuration,
            IUserProvider user,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _user = user;
            _mapper = mapper;
            _jwtManager = new JwtManager();
        }

        /// <inheritdoc />
        public async Task<TokenModel> ExecuteAsync(Param param)
        {
            var userDb = (await _userRepository.FindAsync(t => t.Id == _user.Id)).FirstOrDefault();
            if (userDb == null)
            {
                throw new UnauthorizedException($"Пользователь с Id {_user.Id} не найден.");
            }

            if (userDb.Blocked)
            {
                throw new UnauthorizedException($"Этот пользователь был заблокирован.");
            }

            var user = _mapper.Map<UserModel>(userDb);
            var token = new TokenModel()
            {
                Token = _jwtManager.CreateToken(_configuration, user),
                RefreshToken = _jwtManager.CreateRefreshToken(_configuration, user)
            };

            return token;
        }

        public class Param { }
    }
}
