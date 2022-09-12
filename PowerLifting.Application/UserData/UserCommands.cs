using AutoMapper;
using Microsoft.Extensions.Configuration;
using PowerLifting.Application.UserData.Auth;
using PowerLifting.Application.UserData.Auth.Errors;
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
        private readonly ICrudRepo<UserInfoDb> _userInfoRepository;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public UserCommands(
            ICrudRepo<UserDb> userRepository,
            ICrudRepo<UserInfoDb> userInfoRepository,
            IConfiguration configuration,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _userInfoRepository = userInfoRepository;
            _configuration = configuration;
            _mapper = mapper;
        }

        /// <inheritdoc />
        public async Task<string> LoginAsync(LoginModel loginAuth)
        {
            var userDb = (await _userRepository.FindAsync(t => t.Email == loginAuth.Login)).FirstOrDefault();
            if (userDb == null)
            {
                throw new LoginException("Пользователь с указанным логином не найден.");
            }

            var saltedPassword = PasswordManager.ApplySalt(loginAuth.Password, userDb.Salt);
            if (saltedPassword != userDb.Password)
            {
                throw new LoginException("Пароль указан не верно.");
            }

            var userInfoDb = (await _userInfoRepository.FindAsync(t => t.UserId == userDb.Id)).FirstOrDefault();

            var user = _mapper.Map<User>(userDb);
            var userInfo = _mapper.Map<UserInfo>(userInfoDb);

            string token = JwtManager.CreateToken(_configuration, user, userInfo);
            return token;
        }

        /// <inheritdoc />
        public async Task<string> RegisterAsync(RegistrationModel registerAuth)
        {
            if (registerAuth.Password != registerAuth.PasswordConfirm)
            {
                throw new RegistrationException("Пароль и подтверждение пароля не совпадают.");
            }

            var userDb = (await _userRepository.FindAsync(t => t.Email == registerAuth.Login)).FirstOrDefault();
            if (userDb != null)
            {
                throw new RegistrationException("Пользователь с указанным логином уже существует.");
            }

            string salt = PasswordManager.GenerateSalt();
            userDb = new UserDb()
            {
                Email = registerAuth.Login,
                Salt = salt,
                Password = PasswordManager.ApplySalt(registerAuth.Password, salt),
            };
            await _userRepository.CreateAsync(userDb);

            var user = _mapper.Map<User>(userDb);
            var token = JwtManager.CreateToken(_configuration, user);
            return token;
        }

        /// <inheritdoc />
        public async Task<string> ChangePasswordAsync(RegistrationModel registerAuth)
        {
            if (registerAuth.Password != registerAuth.PasswordConfirm)
            {
                throw new RegistrationException("Пароль и подтверждение пароля не совпадают.");
            }

            var userDb = (await _userRepository.FindAsync(t => t.Email == registerAuth.Login)).FirstOrDefault();
            if (userDb == null)
            {
                throw new RegistrationException("Пользователь с указанным логином не найден.");
            }

            var saltedPassword = PasswordManager.ApplySalt(registerAuth.OldPassword, userDb.Salt);
            if (saltedPassword != userDb.Password)
            {
                throw new LoginException("Пароль указан не верно.");
            }

            string salt = PasswordManager.GenerateSalt();
            userDb.Salt = salt;
            userDb.Salt = PasswordManager.ApplySalt(registerAuth.Password, salt);

            await _userRepository.UpdateAsync(userDb);

            var user = _mapper.Map<User>(userDb);
            var token = JwtManager.CreateToken(_configuration, user);
            return token;
        }
    }
}
