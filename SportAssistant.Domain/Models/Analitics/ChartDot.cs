namespace SportAssistant.Domain.Models.Analitics;

/// <summary>
/// DTO. Модель для X оси (горизонтальная) в Rechart.
/// </summary>
public class ChartDot
{
    /// <summary>
    /// Ключевое поле оси Х в Rechart. Пишем сюда дату, конвертированную в строку нужного формата.
    /// </summary>
    public string Name { get; set; } = string.Empty;
}
