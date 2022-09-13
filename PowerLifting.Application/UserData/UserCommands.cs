using AutoMapper;
using Microsoft.Extensions.Configuration;
using PowerLifting.Application.UserData.Auth;
using PowerLifting.Application.UserData.Auth.Interfaces;
using PowerLifting.Domain.CustomExceptions;
using PowerLifting.Domain.DbModels.UserData;
using PowerLifting.Domain.Interfaces.Common.Repositories;
using PowerLifting.Domain.Interfaces.UserData.Application;
using PowerLifting.Domain.Models.Auth;
using PowerLifting.Domain.Models.UserData;
using PowerLifting.Domain.Models.UserData.Auth;

namespace PowerLifting.Application.UserData
{
    public class UserCommands : IUserCommands
    {
        private readonly ICrudRepo<UserDb> _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IUserProvider _user;
        private readonly IMapper _mapper;

        public UserCommands(
            ICrudRepo<UserDb> userRepository,
            IConfiguration configuration,
            IUserProvider user,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _user = user;
            _mapper = mapper;
        }

        /// <inheritdoc />
        public async Task<TokenModel> LoginAsync(LoginModel loginAuth)
        {
            if (string.IsNullOrEmpty(loginAuth.Login))
            {
                throw new BusinessExceptions("Логин не задан.");
            }
            if (string.IsNullOrEmpty(loginAuth.Password))
            {
                throw new BusinessExceptions("Пароль не задан.");
            }

            var userDb = (await _userRepository.FindAsync(t => t.Email == loginAuth.Login)).FirstOrDefault();
            if (userDb == null)
            {
                throw new BusinessExceptions("Пользователь с указанным логином не найден.");
            }

            var saltedPassword = PasswordManager.ApplySalt(loginAuth.Password, userDb.Salt);
            if (saltedPassword != userDb.Password)
            {
                throw new BusinessExceptions("Пароль указан не верно.");
            }

            var user = _mapper.Map<UserModel>(userDb);
            var token = new TokenModel()
            {
                Token = JwtManager.CreateToken(_configuration, user),
                RefreshToken = JwtManager.CreateRefreshToken(_configuration, user)
            };

            return token;
        }

        /// <inheritdoc />
        public async Task<TokenModel> RegisterAsync(RegistrationModel registerAuth)
        {
            if (string.IsNullOrEmpty(registerAuth.Login))
            {
                throw new BusinessExceptions("Логин не задан.");
            }
            if (string.IsNullOrEmpty(registerAuth.Password))
            {
                throw new BusinessExceptions("Пароль не задан.");
            }

            if (registerAuth.Password != registerAuth.PasswordConfirm)
            {
                throw new BusinessExceptions("Пароль и подтверждение пароля не совпадают.");
            }

            var userDb = (await _userRepository.FindAsync(t => t.Email == registerAuth.Login)).FirstOrDefault();
            if (userDb != null)
            {
                throw new BusinessExceptions("Пользователь с указанным логином уже существует.");
            }

            string salt = PasswordManager.GenerateSalt();
            userDb = new UserDb()
            {
                Email = registerAuth.Login,
                Salt = salt,
                Password = PasswordManager.ApplySalt(registerAuth.Password, salt),
            };
            await _userRepository.CreateAsync(userDb);

            var user = _mapper.Map<UserModel>(userDb);
            var token = new TokenModel()
            {
                Token = JwtManager.CreateToken(_configuration, user),
                RefreshToken = JwtManager.CreateRefreshToken(_configuration, user)
            };

            return token;
        }

        /// <inheritdoc />
        public async Task ChangePasswordAsync(RegistrationModel registerAuth)
        {
            if (string.IsNullOrEmpty(registerAuth.Password))
            {
                throw new BusinessExceptions("Пароль не задан.");
            }

            if (registerAuth.Password != registerAuth.PasswordConfirm)
            {
                throw new BusinessExceptions("Пароль и подтверждение пароля не совпадают.");
            }

            var userDb = (await _userRepository.FindAsync(t => t.Email == registerAuth.Login)).FirstOrDefault();
            if (userDb == null)
            {
                throw new BusinessExceptions("Пользователь с указанным логином не найден.");
            }

            var saltedPassword = PasswordManager.ApplySalt(registerAuth.OldPassword, userDb.Salt);
            if (saltedPassword != userDb.Password)
            {
                throw new BusinessExceptions("Пароль указан не верно.");
            }

            string salt = PasswordManager.GenerateSalt();
            userDb.Salt = salt;
            userDb.Password = PasswordManager.ApplySalt(registerAuth.Password, salt);

            await _userRepository.UpdateAsync(userDb);
        }

        /// <inheritdoc />
        public async Task<TokenModel> RefreshTokenAsync()
        {
            var userDb = (await _userRepository.FindAsync(t => t.Id == _user.Id)).FirstOrDefault();
            if (userDb == null)
            {
                throw new UnauthorizedException($"Пользователь с Id {_user.Id} не найден.");
            }

            var user = _mapper.Map<UserModel>(userDb);
            var token = new TokenModel()
            {
                Token = JwtManager.CreateToken(_configuration, user),
                RefreshToken = JwtManager.CreateRefreshToken(_configuration, user)
            };

            return token;
        }
    }
}
