using PowerLifting.Domain.CustomExceptions;
using System.Net.Mail;

namespace PowerLifting.Application.UserData.Auth
{
    public class AuthDataVaidation
    {
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
