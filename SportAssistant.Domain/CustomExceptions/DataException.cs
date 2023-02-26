namespace SportAssistant.Domain.CustomExceptions;

[Serializable]
public class DataException : Exception
{
    public DataException(string message = "У вас нет прав на просмотр данной информации.") : base(message) { }

    public DataException(string message, Exception innerException) : base(message, innerException) { }
}
