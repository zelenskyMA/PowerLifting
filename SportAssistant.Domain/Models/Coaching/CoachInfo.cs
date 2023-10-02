using SportAssistant.Domain.Models.UserData;

namespace SportAssistant.Domain.Models.Coaching;

public class CoachInfo
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? Age { get; set; }

    public Contacts Contacts { get; set; } = new Contacts();
}
