namespace SportAssistant.Domain.DbModels.Common;

public class NamedEntityDb : EntityDb
{
    public string? Name { get; set; }

    public string? Description { get; set; }
}
