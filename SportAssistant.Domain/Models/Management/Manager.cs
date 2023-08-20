namespace SportAssistant.Domain.Models.Management;

public class Manager
{
    /// <summary>
    /// Ид пользователя
    /// </summary>
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string TelNumber { get; set; } = string.Empty;

    public int AllowedCoaches { get; set; }

    public int OrgId { get; set; }

    public int DistributedCoaches { get; set; }
}
