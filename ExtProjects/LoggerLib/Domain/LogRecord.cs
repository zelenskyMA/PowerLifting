namespace LoggerLib.Domain;

public class LogRecord
{
    /// <summary>
    /// Ид пользователя, вызвавшего эндпоинт.
    /// </summary>
    public int CallerId { get; set; }

    /// <summary>
    /// Адрес вызванного апи.
    /// </summary>
    public string Path { get; set; } = string.Empty;

    public string Method { get; set; } = string.Empty;

    /// <summary>
    /// Запрос к эндпоинту. Десериализованный.
    /// </summary>
    public string Request { get; set; } = string.Empty;

    /// <summary>
    /// Ответ эндпоинта. Десериализованный.
    /// </summary>
    public string Response { get; set; } = string.Empty;

    /// <summary>
    /// Ошибка при получении ответа, если была.
    /// </summary>
    public string Error { get; set; } = string.Empty;

    /// <summary>
    /// Стек ошибки, если был.
    /// </summary>
    public string? StackTrace { get; set; }
}
