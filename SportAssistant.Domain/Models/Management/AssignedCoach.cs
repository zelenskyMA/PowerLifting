using SportAssistant.Domain.Models.UserData;

namespace SportAssistant.Domain.DbModels.Management;

public class AssignedCoach
{
    public int Id => CoachId;

    public int ManagerId { get; set; }

    public int CoachId { get; set; }

    public string CoachName { get; set; } = string.Empty;

    public string ManagerName { get; set; } = string.Empty;

    public string ManagerTel { get; set; } = string.Empty;

    public List<UserInfo> Sportsmen { get; set; } = new List<UserInfo>();
}
