namespace SportAssistant.Domain.CustomExceptions;

[Serializable]
public class BadRequestException : Exception
{
public BadRequestException() { }

public BadRequestException(string message) : base(message) { }

public BadRequestException(string message, Exception innerException) : base(message, innerException) { }
}
