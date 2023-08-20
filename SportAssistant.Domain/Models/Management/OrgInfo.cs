namespace SportAssistant.Domain.Models.Management;

public class OrgInfo
{
    /// <summary>
    /// Кол-во менеджеров в организации
    /// </summary>
    public int ManagerCount { get; set; }

    /// <summary>
    /// Кол-во лицензий, распределенных между менеджерами
    /// </summary>
    public int DistributedLicences { get; set; }

    /// <summary>
    /// Кол-во лицензий , оставшихся для распределения
    /// </summary>
    public int LeftToDistribute { get; set; }

    /// <summary>
    /// Кол-во тренеров, на которых менеджеры потратили лицензии
    /// </summary>
    public int UsedLicenses { get; set; }
}
