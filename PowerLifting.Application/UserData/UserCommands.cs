using AutoMapper;
using Microsoft.Extensions.Configuration;
using PowerLifting.Application.UserData.Auth;
using PowerLifting.Application.UserData.Auth.Interfaces;
using PowerLifting.Domain.CustomExceptions;
using PowerLifting.Domain.DbModels.UserData;
using PowerLifting.Domain.Enums;
using PowerLifting.Domain.Interfaces;
using PowerLifting.Domain.Interfaces.Common.Repositories;
using PowerLifting.Domain.Interfaces.UserData.Application;
using PowerLifting.Domain.Models.Auth;
using PowerLifting.Domain.Models.UserData;
using PowerLifting.Domain.Models.UserData.Auth;

namespace PowerLifting.Application.UserData
{
    public class UserCommands : IUserCommands
    {
        private readonly IUserBlockCommands _userBlockCommands;

        private readonly ICrudRepo<UserDb> _userRepository;
        private readonly ICrudRepo<UserInfoDb> _userInfoRepository;
        private readonly IConfiguration _configuration;
        private readonly IUserProvider _user;
        private readonly IMapper _mapper;

        private readonly AuthDataVaidation _authValidation;
        private readonly PasswordManager _passwordManager;
        private readonly JwtManager _jwtManager;

        public UserCommands(
            IUserBlockCommands userBlockCommands,
            ICrudRepo<UserDb> userRepository,
            ICrudRepo<UserInfoDb> userInfoRepository,
            IConfiguration configuration,
            IUserProvider user,
            IMapper mapper)
        {
            _userBlockCommands = userBlockCommands;

            _userRepository = userRepository;
            _userInfoRepository = userInfoRepository;
            _configuration = configuration;
            _user = user;
            _mapper = mapper;

            _authValidation = new AuthDataVaidation();
            _passwordManager = new PasswordManager();
            _jwtManager = new JwtManager();
        }

        /// <inheritdoc />
        public async Task<TokenModel> LoginAsync(LoginModel loginAuth)
        {
            _authValidation.ValidateLogin(loginAuth.Login);
            _authValidation.ValidatePassword(loginAuth.Password);

            var userDb = await TryToLogin(loginAuth.Login, loginAuth.Password);

            var user = _mapper.Map<UserModel>(userDb);
            var token = new TokenModel()
            {
                Token = _jwtManager.CreateToken(_configuration, user),
                RefreshToken = _jwtManager.CreateRefreshToken(_configuration, user)
            };

            return token;
        }

        /// <inheritdoc />
        public async Task<TokenModel> RegisterAsync(RegistrationModel registerAuth)
        {
            _authValidation.ValidateLogin(registerAuth.Login);
            _authValidation.ValidatePassword(registerAuth.Password, registerAuth.PasswordConfirm, true);

            var userDb = (await _userRepository.FindAsync(t => t.Email == registerAuth.Login)).FirstOrDefault();
            if (userDb != null)
            {
                throw new BusinessException("Пользователь с указанным логином уже существует.");
            }

            var user = await CreateNewUser(registerAuth);

            var token = new TokenModel()
            {
                Token = _jwtManager.CreateToken(_configuration, user),
                RefreshToken = _jwtManager.CreateRefreshToken(_configuration, user)
            };

            return token;
        }

        /// <inheritdoc />
        public async Task ChangePasswordAsync(RegistrationModel registerAuth)
        {
            _authValidation.ValidateLogin(registerAuth.Login);
            _authValidation.ValidatePassword(registerAuth.Password, registerAuth.PasswordConfirm, true);

            var userDb = await TryToLogin(registerAuth.Login, registerAuth.OldPassword);

            string salt = _passwordManager.GenerateSalt();
            userDb.Salt = salt;
            userDb.Password = _passwordManager.ApplySalt(registerAuth.Password, salt);

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

        private async Task<UserDb> TryToLogin(string login, string password)
        {
            var userDb = (await _userRepository.FindAsync(t => t.Email == login)).FirstOrDefault();
            if (userDb == null)
            {
                throw new BusinessException("Пользователь с указанным логином не найден.");
            }

            var saltedPassword = _passwordManager.ApplySalt(password, userDb.Salt);
            if (saltedPassword != userDb.Password)
            {
                throw new BusinessException("Пароль указан не верно.");
            }

            if (userDb.Blocked)
            {
                var blockReson = await _userBlockCommands.GetCurrentBlockReason(userDb.Id);
                throw new UnauthorizedException($"Пользователь заблокирован, обратитесь к администрации. Причина: '{blockReson.Reason}'");
            }

            return userDb;
        }

        private async Task<UserModel> CreateNewUser(RegistrationModel registerAuth)
        {
            string salt = _passwordManager.GenerateSalt();
            var userDb = new UserDb()
            {
                Email = registerAuth.Login,
                Salt = salt,
                Password = _passwordManager.ApplySalt(registerAuth.Password, salt),
                Blocked = false
            };

            await _userRepository.CreateAsync(userDb);
            await _userInfoRepository.CreateAsync(new UserInfoDb() { UserId = userDb.Id });

            return _mapper.Map<UserModel>(userDb);
        }
    }
}
