namespace PowerLifting.Domain.CustomExceptions
{
    [Serializable]
    public class RoleException : Exception
    {
        public RoleException(string message = "У вас нет прав на выполнение данной операции.") : base(message) { }

        public RoleException(string message, Exception innerException) : base(message, innerException) { }
    }
}
