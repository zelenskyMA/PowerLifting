namespace SportAssistant.Domain.DbModels.Management;

public class Manager
{
    public int UserId { get; set; }

    public string TelNumber { get; set; }

    public int AllowedCoaches { get; set; }

    public int OrgId { get; set; }

    public int AssignedCoaches { get; set; }
}
