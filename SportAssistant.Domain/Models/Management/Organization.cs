using SportAssistant.Domain.Models.Common;

namespace SportAssistant.Domain.DbModels.Management;

public class Organization : NamedEntity
{   
    public int OwnerId { get; set; }

    public string? OwnerLegalName { get; set; }

    public int MaxCoaches { get; set; }
}
