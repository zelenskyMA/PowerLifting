using AutoMapper;
using PowerLifting.Application.UserData.Auth;
using PowerLifting.Domain.CustomExceptions;
using PowerLifting.Domain.DbModels.UserData;
using PowerLifting.Domain.Interfaces.Common.Repositories;
using PowerLifting.Domain.Interfaces.UserData.Application;
using PowerLifting.Domain.Models.UserData;
using PowerLifting.Infrastructure.Setup;
using System.Net.Mail;

namespace PowerLifting.Application.UserData.UserCommands
{
    public class ProcessUser : IProcessUser
    {
        private readonly IUserBlockCommands _userBlockCommands;
        private readonly IContextProvider _provider;

        private readonly ICrudRepo<UserDb> _userRepository;
        private readonly ICrudRepo<UserInfoDb> _userInfoRepository;
        private readonly IMapper _mapper;
        private readonly PasswordManager _passwordManager;

        public ProcessUser(
            IUserBlockCommands userBlockCommands,
            IContextProvider provider,
            ICrudRepo<UserDb> userRepository,
            ICrudRepo<UserInfoDb> userInfoRepository,
            IMapper mapper)
        {
            _userBlockCommands = userBlockCommands;
            _provider = provider;

            _userRepository = userRepository;
            _userInfoRepository = userInfoRepository;
            _mapper = mapper;

            _passwordManager = new PasswordManager();
        }              

        /// <inheritdoc />
        public async Task<UserDb> TryToLogin(string login, string password)
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
                throw new BusinessException($"Пользователь заблокирован, обратитесь к администрации. Причина: '{blockReson.Reason}'");
            }

            return userDb;
        }

        /// <inheritdoc />
        public async Task<UserModel> CreateNewUser(string login, string password)
        {
            string salt = _passwordManager.GenerateSalt();
            var userDb = new UserDb()
            {
                Email = login,
                Salt = salt,
                Password = _passwordManager.ApplySalt(password, salt),
                Blocked = false
            };

            await _userRepository.CreateAsync(userDb);

            await _provider.AcceptChangesAsync();

            await _userInfoRepository.CreateAsync(new UserInfoDb() { UserId = userDb.Id });

            return _mapper.Map<UserModel>(userDb);
        }

        /// <inheritdoc />
        public void ValidateLogin(string login)
        {
            if (string.IsNullOrEmpty(login))
            {
                throw new BusinessException("Логин не указан.");
            }

            try
            {
                var addr = new MailAddress(login);
                if (addr.Address != login)
                {
                    throw new BusinessException();
                }
            }
            catch
            {
                throw new BusinessException("Формат логина не соответствует почте.");
            }
        }

        /// <inheritdoc />
        public void ValidatePassword(string password, string confirm = "", bool checkConfirmation = false)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new BusinessException("Пароль не указан.");
            }

            if (password.Length < 6)
            {
                throw new BusinessException("Слишком короткий пароль. Минимум 6 символов");
            }

            if (checkConfirmation && password != confirm)
            {
                throw new BusinessException("Пароль и подтверждение пароля не совпадают.");
            }
        }
    }
}
