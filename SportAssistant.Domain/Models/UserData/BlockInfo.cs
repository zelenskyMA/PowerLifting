namespace SportAssistant.Domain.Models.UserData;

/// <summary>
/// Model for UI to supply info about user blocking action
/// </summary>
public class BlockInfo
{
    public int UserId { get; set; }

    public bool Status { get; set; }

    public string? Reason { get; set; }
}
