namespace PowerLifting.Domain.CustomExceptions
{
    [Serializable]
    public class BusinessExceptions : Exception
    {
        public BusinessExceptions() { }

        public BusinessExceptions(string message) : base(message) { }

        public BusinessExceptions(string message, Exception innerException) : base(message, innerException) { }
    }
}
