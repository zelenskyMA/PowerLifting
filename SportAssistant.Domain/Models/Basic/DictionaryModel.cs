using SportAssistant.Domain.Models.Common;

namespace SportAssistant.Domain.Models;

public class DictionaryItem : NamedEntity
{
    public int TypeId { get; set; }

    public string? TypeName { get; set; }
}
