using System.ComponentModel.DataAnnotations.Schema;

namespace SportAssistant.Domain.DbModels.Management;

[Table("AssignedCoaches", Schema = "org")]
public class AssignedCoachDb
{
    public int ManagerId { get; set; }

    public int CoachId { get; set; }
}
