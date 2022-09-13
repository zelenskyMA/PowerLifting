using AutoMapper;
using Microsoft.Extensions.Configuration;
using PowerLifting.Application.UserData.Auth;
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
        private readonly IMapper _mapper;

        public UserCommands(
            ICrudRepo<UserDb> userRepository,
            IConfiguration configuration,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _mapper = mapper;
        }

        /// <inheritdoc />
        public async Task<string> LoginAsync(LoginModel loginAuth)
        {
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

            string token = JwtManager.CreateToken(_configuration, _mapper.Map<UserModel>(userDb));

            return token;
        }

        /// <inheritdoc />
        public async Task<string> RegisterAsync(RegistrationModel registerAuth)
        {
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
            var token = JwtManager.CreateToken(_configuration, user);
            return token;
        }

        /// <inheritdoc />
        public async Task<string> ChangePasswordAsync(RegistrationModel registerAuth)
        {
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

            var user = _mapper.Map<UserModel>(userDb);
            var token = JwtManager.CreateToken(_configuration, user);
            return token;
        }
    }
}
