using AutoMapper;
using Microsoft.Extensions.Configuration;
using PowerLifting.Application.UserData.Auth;
using PowerLifting.Application.UserData.Auth.Interfaces;
using PowerLifting.Domain.CustomExceptions;
using PowerLifting.Domain.DbModels.UserData;
using PowerLifting.Domain.Interfaces.Common.Operations;
using PowerLifting.Domain.Interfaces.Common.Repositories;
using PowerLifting.Domain.Models.UserData;
using PowerLifting.Domain.Models.UserData.Auth;

namespace PowerLifting.Application.UserData.UserCommands
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
