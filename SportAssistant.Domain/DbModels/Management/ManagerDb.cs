using System.ComponentModel.DataAnnotations.Schema;

namespace SportAssistant.Domain.DbModels.Management;

[Table("Managers", Schema = "org")]
public class ManagerDb
{
    public int UserId { get; set; }

    public string? TelNumber { get; set; }

    public int AllowedCoaches { get; set; }

    public int OrgId { get; set; }
}
