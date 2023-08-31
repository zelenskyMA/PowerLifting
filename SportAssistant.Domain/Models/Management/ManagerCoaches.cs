namespace SportAssistant.Domain.Models.Management;

public class ManagerCoaches
{
    public int ManagerId { get; set; }

    public List<int> CoachIds { get; set; } = new List<int>();
}
