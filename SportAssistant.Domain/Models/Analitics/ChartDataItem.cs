namespace SportAssistant.Domain.Models.Analitics;

/// <summary>
/// DTO. Модель с данными для линии графика в Rechart.
/// </summary>
public class ChartDataItem
{
    /// <summary>
    /// Ключевое поле для внутренних данных.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Наименование линии графика.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Список пар ключ-значени (X-Y), формирующих линию графика.
    /// </summary>
    public List<KvModel> Data { get; set; } = new List<KvModel>();


    /// <summary>
    /// Элемент списка значений линии графика
    /// </summary>
    public class KvModel
    {
        /// <summary>
        /// Ключевое поле оси Х в Rechart. Пишем сюда дату, конвертированную в строку нужного формата.
        /// Используем для выбора значений по ключу.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Содержит значение по оси Y
        /// </summary>
        public int Value { get; set; } = 0;
    }
}