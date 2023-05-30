using SportAssistant.Domain.Models.Common;

namespace SportAssistant.Domain.Models.Coaching;

public class TrainingRequest : Entity
{
    public int UserId { get; set; }

    public int CoachId { get; set; }

    public string? CoachName { get; set; }

    public string? UserName { get; set; }

    public DateTime CreationDate { get; set; }

    public int UserAge { get; set; }

    public int UserWeight { get; set; }

    public int UserHeight { get; set; }
}
